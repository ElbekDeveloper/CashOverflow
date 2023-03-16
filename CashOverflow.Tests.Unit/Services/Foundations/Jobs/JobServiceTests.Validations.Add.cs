// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Threading.Tasks;
using CashOverflow.Models.Jobs;
using CashOverflow.Models.Jobs.Exceptions;
using CashOverflow.Models.Locations.Exceptions;
using FluentAssertions;
using Moq;
using Tynamix.ObjectFiller;
using Xunit;

namespace CashOverflow.Tests.Unit.Services.Foundations.Jobs
{
	public partial class JobServiceTests
	{
		[Fact]
		public async Task ShouldThrowValidationExceptionOnAddIfInputIsNullAndLogItAsync()
		{
			// given
			Job nullJob = null;
			var nullJobException = new NullJobException();
			var expectedJobValidationException =
				new JobValidationException(nullJobException);

			// when
			ValueTask<Job> addJobTask = this.jobService.AddJobAsync(nullJob);

			JobValidationException actualJobValidationException =
				await Assert.ThrowsAsync<JobValidationException>(addJobTask.AsTask);

			// then
			actualJobValidationException.Should().BeEquivalentTo(expectedJobValidationException);

			this.loggingBrokerMock.Verify(broker =>
			broker.LogError(It.Is(SameExceptionAs(expectedJobValidationException))), Times.Once);

			this.storageBrokerMock.Verify(broker =>
			broker.InsertJobAsync(It.IsAny<Job>()), Times.Never );

			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
		}

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
		public async Task ShouldThrowValidationExceptionOnAddIfJobIsInvalidAndLogItAsync(
			string invalidText)
		{
			// given

			var invalidJob = new Job
			{
				Title = invalidText
			};

			var invalidJobException = new InvalidJobException();

			invalidJobException.AddData(
				key: nameof(Job.Id),
				values: "Id is required");

            invalidJobException.AddData(
                key: nameof(Job.Title),
                values: "Title is required");

            invalidJobException.AddData(
                key: nameof(Job.Level),
                values: "Level is required");

            invalidJobException.AddData(
                key: nameof(Job.CreatedDate),
                values: "Date is required");

            invalidJobException.AddData(
                key: nameof(Job.UpdatedDate),
                values: "Date is required");

			var expectedJobValidationException =
				new JobValidationException(invalidJobException);

			// when

			ValueTask<Job> AddJobTask = this.jobService.AddJobAsync(invalidJob);

			JobValidationException actualJobValidationException =
				await Assert.ThrowsAsync<JobValidationException>(AddJobTask.AsTask);

			// then
			actualJobValidationException.Should().BeEquivalentTo(expectedJobValidationException);

			this.loggingBrokerMock.Verify(broker =>
			broker.LogError(It.Is(
				SameExceptionAs(actualJobValidationException))), Times.Once);

			this.storageBrokerMock.Verify(broker =>
			broker.InsertJobAsync(It.IsAny<Job>()), Times.Never);

			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
        }

		[Fact]
		public async Task ShouldThrowValidationExceptionOnAddIfCreatedDateIsNotSameAsUpdatedDateAndLogItAsync()
		{
			// given
			int randomMinutes = GetRandomNumber();
			DateTimeOffset randomDate = GetRandomDatetimeOffset();
			Job randomJob = CreateRandomJob(randomDate);
			Job invalidJob = randomJob;
			invalidJob.UpdatedDate = randomDate.AddMinutes(randomMinutes);
			var invalidJobException = new InvalidJobException();

			invalidJobException.AddData(
				key: nameof(Job.CreatedDate),
				values: $"Date is not the same as {nameof(Job.UpdatedDate)}");

			var expectedJobValidationException = new JobValidationException(invalidJobException);

			// when
			ValueTask<Job> actualJobTask = this.jobService.AddJobAsync(invalidJob);

			JobValidationException actualJobValidationException=
				await Assert.ThrowsAsync<JobValidationException>(actualJobTask.AsTask);

			// then
			actualJobValidationException.Should().BeEquivalentTo(expectedJobValidationException);

			this.loggingBrokerMock.Verify(broker =>
			broker.LogError(It.Is(SameExceptionAs(expectedJobValidationException))), Times.Once);

			this.storageBrokerMock.Verify(broker => broker.InsertJobAsync(It.IsAny<Job>()), Times.Never);

			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
			this.dateTimeBrokerMock.VerifyNoOtherCalls();
		}

		[Theory]
		[MemberData(nameof(InvalidMinutes))]
        public async Task ShouldThrowValidationExceptionOnAddIfCreatedDateIsNotRecentAndLogItAsync(
			int invalidMinutes)
		{
            // given
            DateTimeOffset randomDate = GetRandomDatetimeOffset();
            DateTimeOffset invalidDateTime = randomDate.AddMinutes(invalidMinutes);
            Job randomJob = CreateRandomJob(invalidDateTime);
            Job invalidJob = randomJob;

            var invalidJobException = new InvalidJobException();

            invalidJobException.AddData(
                key: nameof(Job.CreatedDate),
                values: $"Date is not recent");

			var expectedLocationValidationException = new JobValidationException(invalidJobException);

			this.dateTimeBrokerMock.Setup(broker =>
			broker.GetCurrentDateTimeOffset()).Returns(randomDate);

			// when
			ValueTask<Job> addJobTask = this.jobService.AddJobAsync(invalidJob);

			JobValidationException actualJobValidationException =
				await Assert.ThrowsAsync<JobValidationException>(addJobTask.AsTask);

			// then
			actualJobValidationException.Should().BeEquivalentTo(expectedLocationValidationException);

			this.dateTimeBrokerMock.Verify(broker => broker.GetCurrentDateTimeOffset(), Times.Once);

			this.loggingBrokerMock.Verify(broker =>
			broker.LogError(It.Is(SameExceptionAs(expectedLocationValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker => broker.InsertJobAsync(It.IsAny<Job>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();

        }
    }
}

