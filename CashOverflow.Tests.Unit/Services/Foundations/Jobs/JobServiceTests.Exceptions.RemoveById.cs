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
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace CashOverflow.Tests.Unit.Services.Foundations.Jobs
{
    public partial class JobServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyValidationOnRemoveIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Guid someJobId = Guid.NewGuid();

            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedJobException =
                new LockedJobException(databaseUpdateConcurrencyException);

            var expectedJobDependencyValidationException =
                new JobDependencyValidationException(lockedJobException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectJobByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Job> removeJobByIdTask =
               this.jobService.RemoveJobByIdAsync(someJobId);

            JobDependencyValidationException actualJobDependencyValidationException =
                await Assert.ThrowsAsync<JobDependencyValidationException>(
                    removeJobByIdTask.AsTask);

            // then
            actualJobDependencyValidationException.Should().BeEquivalentTo(
               expectedJobDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectJobByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedJobDependencyValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteJobAsync(It.IsAny<Job>()), Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBroker.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someJobId = Guid.NewGuid();
            SqlException sqlException = CreateSqlException();

            var failedJobStorageException =
                new FailedJobStorageException(sqlException);

            var expectedJobDependencyException =
                new JobDependencyException(failedJobStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectJobByIdAsync(someJobId))
                    .Throws(sqlException);

            // when
            ValueTask<Job> deleteJobTask =
                this.jobService.RemoveJobByIdAsync(someJobId);

            JobDependencyException actualJobDependencyException =
                    await Assert.ThrowsAsync<JobDependencyException>(
                        deleteJobTask.AsTask);

            // then
            actualJobDependencyException.Should().BeEquivalentTo(expectedJobDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectJobByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedJobDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBroker.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveIfExceptionOccursAndLogItAsync()
        {
            // given
            Guid someJobId = Guid.NewGuid();
            var seviceException = new Exception();

            var failedJobServiceException =
               new FailedJobServiceException(seviceException);

            var expectedJobServiceException =
                new JobServiceException(failedJobServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectJobByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(seviceException);

            // when
            ValueTask<Job> removeJobByIdTask =
                this.jobService.RemoveJobByIdAsync(someJobId);

            JobServiceException actualJobServiceException =
                await Assert.ThrowsAsync<JobServiceException>(
                    removeJobByIdTask.AsTask);

            // then
            actualJobServiceException.Should().BeEquivalentTo(
                expectedJobServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectJobByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedJobServiceException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBroker.VerifyNoOtherCalls();
        }
    }
}
