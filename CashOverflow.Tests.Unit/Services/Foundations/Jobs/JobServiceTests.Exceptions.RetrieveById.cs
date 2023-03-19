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
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
        {
            //given
            Guid someId = Guid.NewGuid();
            SqlException sqlException = CreateSqlException();

            var failedJobStorageException =
                new FailedJobStorageException(sqlException);

            var expectedJobDependencyException =
                new JobDependencyException(failedJobStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectJobByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            //when
            ValueTask<Job> retrieveJobByIdTask =
                this.jobService.RetrieveJobByIdAsync(someId);

            JobDependencyException actualJobDependencyexception =
                await Assert.ThrowsAsync<JobDependencyException>(
                    retrieveJobByIdTask.AsTask);

            //then
            actualJobDependencyexception.Should().BeEquivalentTo(
                expectedJobDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectJobByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedJobDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdAsyncIfServiceErrorOccursAndLogItAsync()
        {
            //given
            Guid someId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedJobServiceException =
                new FailedJobServiceException(serviceException);

            var expectedJobServiceException =
                new JobServiceException(failedJobServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectJobByIdAsync(It.IsAny<Guid>())).ThrowsAsync(serviceException);

            //when
            ValueTask<Job> retrieveJobByIdTask =
                this.jobService.RetrieveJobByIdAsync(someId);

            JobServiceException actualJobServiceException =
                await Assert.ThrowsAsync<JobServiceException>(retrieveJobByIdTask.AsTask);

            //then
            actualJobServiceException.Should().BeEquivalentTo(expectedJobServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectJobByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedJobServiceException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
