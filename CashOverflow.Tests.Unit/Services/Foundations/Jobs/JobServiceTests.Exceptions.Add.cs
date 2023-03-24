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
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace CashOverflow.Tests.Unit.Services.Foundations.Jobs
{
    public partial class JobServiceTests
    {
        [Fact]
        public async Task ShoudlThrowCriticalDependencyExceptionOnAddIfDependencyErrorOccursAndLogItAsync()
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
                broker.LogCritical(It.Is(
                    SameExceptionAs(expectedJobDependencyException))), Times.Once);

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

            var expectedJobDependencyValidationException =
                new JobDependencyValidationException(alreadyExistsJobException);

            this.dateTimeBrokerMock.Setup(broker => broker.GetCurrentDateTimeOffset())
                .Throws(duplicateKeyException);

            // when
            ValueTask<Job> addJobTask = this.jobService.AddJobAsync(someJob);

            JobDependencyValidationException actualJobDependencyValidationException =
                await Assert.ThrowsAsync<JobDependencyValidationException>(addJobTask.AsTask);

            // then
            actualJobDependencyValidationException.Should()
                .BeEquivalentTo(expectedJobDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker => broker.LogError(It.Is(
                SameExceptionAs(expectedJobDependencyValidationException))), Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfDbConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Job someJob = CreateRandomJob();
            var dbUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedJobException = new LockedJobException(dbUpdateConcurrencyException);

            var expectedJobDependencyValidationException =
                new JobDependencyValidationException(lockedJobException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(dbUpdateConcurrencyException);

            // when
            ValueTask<Job> addJobTask = this.jobService.AddJobAsync(someJob);

            JobDependencyValidationException actualJobDependencyValidationException =
                 await Assert.ThrowsAsync<JobDependencyValidationException>(addJobTask.AsTask);

            // then
            actualJobDependencyValidationException.Should()
                .BeEquivalentTo(expectedJobDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker => broker.LogError(It.Is(
                SameExceptionAs(expectedJobDependencyValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertJobAsync(It.IsAny<Job>()), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddIfDbUpdateErrorOccursAndLogItAsync()
        {
            // given
            Job someJob = CreateRandomJob();
            var dbUpdateException = new DbUpdateException();

            var failedJobStorageException = new FailedJobStorageException(dbUpdateException);

            var expectedJobDependencyException =
                new JobDependencyException(failedJobStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(dbUpdateException);

            // when
            ValueTask<Job> addJobTask = this.jobService.AddJobAsync(someJob);

            JobDependencyException actualJobDependencyException =
                 await Assert.ThrowsAsync<JobDependencyException>(addJobTask.AsTask);

            // then
            actualJobDependencyException.Should()
                .BeEquivalentTo(expectedJobDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker => broker.LogError(It.Is(
                SameExceptionAs(expectedJobDependencyException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateJobAsync(It.IsAny<Job>()), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Job someJob = new Job();
            var serviceException = new Exception();
            var failedJobServiceException = new FailedJobServiceException(serviceException);

            var expectedJobServiceException =
                new JobServiceException(failedJobServiceException);

            this.dateTimeBrokerMock.Setup(broker => broker.GetCurrentDateTimeOffset())
                .Throws(serviceException);

            // when
            ValueTask<Job> addJobTask = this.jobService.AddJobAsync(someJob);

            JobServiceException actualJobServiceException =
                await Assert.ThrowsAsync<JobServiceException>(addJobTask.AsTask);

            // then
            actualJobServiceException.Should().BeEquivalentTo(expectedJobServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker => broker.LogError(It.Is(SameExceptionAs(
                expectedJobServiceException))), Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}

