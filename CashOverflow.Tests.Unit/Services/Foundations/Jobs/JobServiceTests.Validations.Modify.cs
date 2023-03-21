// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Threading.Tasks;
using CashOverflow.Models.Jobs;
using CashOverflow.Models.Jobs.Exceptions;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Xunit;

namespace CashOverflow.Tests.Unit.Services.Foundations.Jobs
{
    public partial class JobServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfJobIsNullAndLogItAsync()
        {
            // given
            Job nullJob = null;
            var nullJobException = new NullJobException();

            var expectedJobValidationException =
                new JobValidationException(nullJobException);

            // when
            ValueTask<Job> modifyJobTask = this.jobService.ModifyJobAsync(nullJob);

            JobValidationException actualJobValidationException =
                await Assert.ThrowsAsync<JobValidationException>(
                    modifyJobTask.AsTask);

            // then
            actualJobValidationException.Should().BeEquivalentTo(expectedJobValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedJobValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfJobIsInvalidAndLogItAsync(string invalidString)
        {
            // given
            Job invalidJob = new Job
            {
                Title = invalidString
            };

            var invalidJobException = new InvalidJobException();

            invalidJobException.AddData(
                key: nameof(Job.Id),
                values: "Id is required");

            invalidJobException.AddData(
                key: nameof(Job.Title),
                values: "Text is required");

            invalidJobException.AddData(
                key: nameof(Job.CreatedDate),
                values: "Value is required");

            invalidJobException.AddData(
                key: nameof(Job.UpdatedDate),
                values: new[]
                    {
                        "Value is required",
                        "Date is not recent.",
                        $"Date is the same as {nameof(Job.CreatedDate)}"
                    }
                );

            var expectedJobValidationException =
                new JobValidationException(invalidJobException);


            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(GetRandomDateTime);

            // when
            ValueTask<Job> modifyJobTask = this.jobService.ModifyJobAsync(invalidJob);

            JobValidationException actualJobValidationException =
                await Assert.ThrowsAsync<JobValidationException>(
                    modifyJobTask.AsTask);

            // then
            actualJobValidationException.Should().BeEquivalentTo(expectedJobValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedJobValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsNotSameAsCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Job randomJob = CreateRandomJob(randomDateTime);
            Job invalidJob = randomJob;
            var invalidJobException = new InvalidJobException();

            invalidJobException.AddData(
                key: nameof(Job.UpdatedDate),
                values: $"Date is the same as {nameof(Job.CreatedDate)}");

            var expectedJobValidationException =
                new JobValidationException(invalidJobException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Returns(randomDateTime);

            // when
            ValueTask<Job> modifyJobTask =
                this.jobService.ModifyJobAsync(invalidJob);

            JobValidationException actualJobValidationException =
                 await Assert.ThrowsAsync<JobValidationException>(
                    modifyJobTask.AsTask);

            // then
            actualJobValidationException.Should()
                .BeEquivalentTo(expectedJobValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectJobByIdAsync(invalidJob.Id), Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedJobValidationException))), Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidSeconds))]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsNotRecentAndLogItAsync(int minutes)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Job randomJob = CreateRandomJob(dateTime);
            Job inputJob = randomJob;
            inputJob.UpdatedDate = dateTime.AddMinutes(minutes);
            var invalidJobException = new InvalidJobException();

            invalidJobException.AddData(
                key: nameof(Job.UpdatedDate),
                values: "Date is not recent.");

            var expectedJobValidatonException =
                new JobValidationException(invalidJobException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Returns(dateTime);

            // when
            ValueTask<Job> modifyJobTask =
                this.jobService.ModifyJobAsync(inputJob);

            JobValidationException actualJobValidationException =
                await Assert.ThrowsAsync<JobValidationException>(
                    modifyJobTask.AsTask);

            // then
            actualJobValidationException.Should().BeEquivalentTo(expectedJobValidatonException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedJobValidatonException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectJobByIdAsync(It.IsAny<Guid>()), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfTeamDoesNotExistAndLogItAsync()
        {
            // given
            int negativeMinutes = GetRandomNegativeNumber();
            DateTimeOffset dateTime = GetRandomDateTime();
            Job randomJob = CreateRandomJob(dateTime);
            Job nonExistJob = randomJob;
            nonExistJob.CreatedDate = dateTime.AddMinutes(negativeMinutes);
            Job nullJob = null;

            var notFoundJobException = new NotFoundJobException(nonExistJob.Id);

            var expectedJobValidationException =
                new JobValidationException(notFoundJobException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectJobByIdAsync(nonExistJob.Id))
                    .ReturnsAsync(nullJob);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Returns(dateTime);

            // when
            ValueTask<Job> modifyJobTask =
                this.jobService.ModifyJobAsync(nonExistJob);

            JobValidationException actualJobValidationException =
                await Assert.ThrowsAsync<JobValidationException>(
                    modifyJobTask.AsTask);

            // then
            actualJobValidationException.Should().BeEquivalentTo(expectedJobValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectJobByIdAsync(nonExistJob.Id), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedJobValidationException))), Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedDateNotSameAsCreatedDateAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNegativeNumber();
            int randomMinutes = randomNumber;
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Job randomJob = CreateRandomModifyJob(randomDateTime);
            Job invalidJob = randomJob.DeepClone();
            Job storageJob = invalidJob.DeepClone();
            storageJob.CreatedDate = storageJob.CreatedDate.AddMinutes(randomMinutes);
            storageJob.UpdatedDate = storageJob.UpdatedDate.AddMinutes(randomMinutes);
            var invalidJobException = new InvalidJobException();
            Guid jobId = invalidJob.Id;

            invalidJobException.AddData(
                key: nameof(Job.CreatedDate),
                values: $"Date is not same as {nameof(Job.CreatedDate)}");

            var expectedJobValidationException =
                new JobValidationException(invalidJobException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectJobByIdAsync(jobId)).ReturnsAsync(storageJob);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Returns(randomDateTime);

            // when
            ValueTask<Job> modifyJobTask =
                this.jobService.ModifyJobAsync(invalidJob);

            JobValidationException actualJobValidationException =
                await Assert.ThrowsAsync<JobValidationException>(modifyJobTask.AsTask);

            // then
            actualJobValidationException.Should().BeEquivalentTo(expectedJobValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectJobByIdAsync(invalidJob.Id), Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedJobValidationException))), Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageUpdatedDateSameAsUpdatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Job randomJob = CreateRandomModifyJob(randomDateTime);
            Job invalidJob = randomJob;
            Job storageJob = randomJob.DeepClone();
            invalidJob.UpdatedDate = storageJob.UpdatedDate;
            Guid jobId = invalidJob.Id;
            var invalidJobException = new InvalidJobException();

            invalidJobException.AddData(
                key: nameof(Job.UpdatedDate),
                values: $"Data is the same as {nameof(Job.UpdatedDate)}");

            var expectedJobValidationException =
                new JobValidationException(invalidJobException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectJobByIdAsync(invalidJob.Id)).ReturnsAsync(storageJob);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Returns(randomDateTime);

            // when
            ValueTask<Job> modifyJobTask =
                this.jobService.ModifyJobAsync(invalidJob);

            JobValidationException actualJobValidationException =
                await Assert.ThrowsAsync<JobValidationException>(modifyJobTask.AsTask);

            // then
            actualJobValidationException.Should()
                .BeEquivalentTo(expectedJobValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectJobByIdAsync(jobId), Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedJobValidationException))), Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once());

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
