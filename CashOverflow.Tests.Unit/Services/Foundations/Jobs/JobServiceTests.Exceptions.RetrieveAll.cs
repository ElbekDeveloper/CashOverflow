// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
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
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            //given
            SqlException sqlException = CreateSqlException();

            var faildeStorageException =
                new FailedJobStorageException(sqlException);

            var expectedJobDependencyException =
                new JobDependencyException(faildeStorageException);

            this.storageBrokerMock.Setup(broker => 
                broker.SelectAllJobs()).
                    Throws(sqlException);

            //when
            Action retrieveAllJobsAction = () =>
                this.jobService.RetrieveAllJobs();

            JobDependencyException actualJobDependencyException =
                Assert.Throws<JobDependencyException>(retrieveAllJobsAction);

            //then
            actualJobDependencyException.Should().BeEquivalentTo(
                expectedJobDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllJobs(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker => 
                broker.LogCritical(It.Is(SameExceptionAs(expectedJobDependencyException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
