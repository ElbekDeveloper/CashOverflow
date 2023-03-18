// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Threading.Tasks;
using CashOverflow.Models.Jobs;
using CashOverflow.Models.Jobs.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace CashOverflow.Tests.Unit.Services.Foundations.Jobs
{
	public partial class JobServiceTests
	{
		[Fact]
		public async Task ShoudlThrowCriticalDependancyExceptionOnAddIfDependancyErrorOccursAndLogItAsync()
		{
			// given
			Job someJob = CreateRandomJob();
			SqlException sqlException = CreateSqlException();
			FailedJobStorageException failedJobStorageException = new FailedJobStorageException(sqlException);
			var expectedJobDependancyException = new JobDependancyException(failedJobStorageException);

			this.dateTimeBrokerMock.Setup(broker =>
			broker.GetCurrentDateTimeOffset()).Throws(sqlException);

			// when
			ValueTask<Job> addJobTask = this.jobService.AddJobAsync(someJob);

			JobDependancyException actualJobDependancyException =
				await Assert.ThrowsAsync<JobDependancyException>(addJobTask.AsTask);

			// then
			actualJobDependancyException.Should().BeEquivalentTo(expectedJobDependancyException);

			this.dateTimeBrokerMock.Verify(broker => broker.GetCurrentDateTimeOffset(), Times.Once);

			this.loggingBrokerMock.Verify(broker =>
			broker.LogCritical(It.Is(SameExceptionAs(expectedJobDependancyException))), Times.Once);

			this.dateTimeBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
		}
    }
}

