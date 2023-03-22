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
        public async Task ShoudlThrowCriticalDependencyExceptionOnAddIfdependencyErrorOccursAndLogItAsync()
        {
            // given
            Job someJob = CreateRandomJob();
            SqlException sqlException = CreateSqlException();
            var failedJobStorageException = new FailedJobStorageException(sqlException);
            var expectedJobDependencyException = new JobDependencyException(failedJobStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Throws(sqlException);

            // when
            ValueTask<Job> addJobTask = this.jobService.AddJobAsync(someJob);

            JobDependencyException actualJobDependencyException =
                await Assert.ThrowsAsync<JobDependencyException>(addJobTask.AsTask);

            // then
            actualJobDependencyException.Should().BeEquivalentTo(expectedJobDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedJobDependencyException))), Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}

