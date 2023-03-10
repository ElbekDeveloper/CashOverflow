// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Threading.Tasks;
using CashOverflow.Models.Jobs;
using CashOverflow.Models.Jobs.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace CashOverflow.Tests.Unit.Services.Foundations.Jobs
{
    public partial class JobServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveIfIdIsInvalidAndLogItAsync()
        {
            // given
            var invalidJobId = Guid.Empty;

            var invalidJobException = new InvalidJobException();

            invalidJobException.AddData(
                key: nameof(Job.Id),
                values: "Id is required");

            var expectedJobValidationException =
                new JobValidationException(invalidJobException);

            // when
            ValueTask<Job> removeJobByIdTask =
                this.jobService.RemoveJobByIdAsync(invalidJobId);

            JobValidationException actualJobValidationException =
            await Assert.ThrowsAsync<JobValidationException>(
                removeJobByIdTask.AsTask);

            // then
            actualJobValidationException.Should().BeEquivalentTo(expectedJobValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedJobValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBroker.VerifyNoOtherCalls();

        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRemoveIfJobIsNotFoundAndLogItAsync()
        {
            // given
            Guid randomJobId = Guid.NewGuid();
            Guid inputJobId = randomJobId;
            Job noJob = null;

            var notFoundJobException =
                new NotFoundJobException(inputJobId);

            var expectedJobValidationException =
                new JobValidationException(notFoundJobException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectJobByIdAsync(It.IsAny<Guid>())).ReturnsAsync(noJob);

            // when
            ValueTask<Job> removeJobByIdTask =
               this.jobService.RemoveJobByIdAsync(inputJobId);

            JobValidationException actualJobValidationException =
                await Assert.ThrowsAsync<JobValidationException>(
                    removeJobByIdTask.AsTask);

            // then
            actualJobValidationException.Should().BeEquivalentTo(expectedJobValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectJobByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedJobValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteJobAsync(It.IsAny<Job>()), Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBroker.VerifyNoOtherCalls();
        }
    }
}
