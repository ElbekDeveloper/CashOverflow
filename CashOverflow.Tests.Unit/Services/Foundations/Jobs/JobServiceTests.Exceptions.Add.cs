// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Threading.Tasks;
using CashOverflow.Models.Jobs;
using CashOverflow.Models.Jobs.Exceptions;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace CashOverflow.Tests.Unit.Services.Foundations.Jobs
{
	public partial class JobServiceTests
	{
		[Fact]
		public async Task ShoudlThrowCriticalDependencyExceptionOnAddIfdependencyErrorOccursAndLogItAsync()
		{
			// given
			Job someJob = CreateRandomJob();
			SqlException sqlException = CreateSqlException();
			var failedJobStorageException = new FailedJobStorageException(sqlException);
			var expectedJobdependencyException = new JobDependencyException(failedJobStorageException);

			this.dateTimeBrokerMock.Setup(broker =>
			broker.GetCurrentDateTimeOffset()).Throws(sqlException);

			// when
			ValueTask<Job> addJobTask = this.jobService.AddJobAsync(someJob);

			JobDependencyException actualJobdependencyException =
				await Assert.ThrowsAsync<JobDependencyException>(addJobTask.AsTask);

			// then
			actualJobdependencyException.Should().BeEquivalentTo(expectedJobdependencyException);

			this.dateTimeBrokerMock.Verify(broker => broker.GetCurrentDateTimeOffset(), Times.Once);

			this.loggingBrokerMock.Verify(broker =>
			broker.LogCritical(It.Is(SameExceptionAs(expectedJobdependencyException))), Times.Once);

			this.dateTimeBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
		}

		[Fact]
		public async Task ShouldThrowDependencyValidationExceptionOnAddIfDublicateKeyErrorOccursAndLogItAsync()
		{
			// given
			string someMessage = GetRandomString();
			Job someJob = new Job();
			var duplicateKeyException = new DuplicateKeyException(someMessage);
			var alreadyExistsJobException = new AlreadyExistsJobException(duplicateKeyException);
			var expectedJobDependencyValidationException = new JobDependencyValidationException(alreadyExistsJobException);

			this.dateTimeBrokerMock.Setup(broker => broker.GetCurrentDateTimeOffset())
				.Throws(duplicateKeyException);

			// when
			ValueTask<Job> addJobTask = this.jobService.AddJobAsync(someJob);

			JobDependencyValidationException actualJobDependencyValidationException =
				await Assert.ThrowsAsync<JobDependencyValidationException>(addJobTask.AsTask);

			// then
			actualJobDependencyValidationException.Should()
				.BeEquivalentTo(expectedJobDependencyValidationException);

			this.dateTimeBrokerMock.Verify(broker => broker.GetCurrentDateTimeOffset(), Times.Once);

			this.loggingBrokerMock.Verify(broker => broker.LogError(It.Is(
				SameExceptionAs(expectedJobDependencyValidationException))), Times.Once);

			this.dateTimeBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
		}
    }
}

