// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Diagnostics;
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
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            DateTimeOffset someDateTime = GetRandomDateTime();
            Job randomJob = CreateRandomJob();
            Job someJob = randomJob;
            Guid jobId = someJob.Id;
            SqlException sqlException = CreateSqlException();

            var failedJobStorageException =
                new FailedJobStorageException(sqlException);

            var expectedJobDependencyException =
                new JobDependencyException(failedJobStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Throws(sqlException);

            // when
            ValueTask<Job> modifyJobTask =
                this.jobService.ModifyJobAsync(someJob);

            JobDependencyException actualJobDependencyException =
                await Assert.ThrowsAsync<JobDependencyException>(
                    modifyJobTask.AsTask);

            // then
            actualJobDependencyException.Should().BeEquivalentTo(
                expectedJobDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectJobByIdAsync(jobId), Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateJobAsync(someJob), Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedJobDependencyException))), Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
               broker.GetCurrentDateTimeOffset(), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            int minutesInPast = GetRandomNegativeNumber();
            DateTimeOffset randomDateTime = GetRandomDatetimeOffset();
            Job randomJob = CreateRandomJob(randomDateTime);
            Job someJob = randomJob;
            Guid jobId = someJob.Id;
            someJob.CreatedDate = someJob.CreatedDate.AddMinutes(minutesInPast);
            var databaseUpdateException = new DbUpdateException();

            var failedStorageJobException =
                new FailedJobStorageException(databaseUpdateException);

            var expectedJobDependencyException =
                new JobDependencyException(failedStorageJobException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectJobByIdAsync(jobId))
                    .ThrowsAsync(databaseUpdateException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTime);

            // when
            ValueTask<Job> modifyJobTask =
                this.jobService.ModifyJobAsync(someJob);

            JobDependencyException actualJobDependencyException =
                await Assert.ThrowsAsync<JobDependencyException>(
                    modifyJobTask.AsTask);

            // then
            actualJobDependencyException.Should().BeEquivalentTo(expectedJobDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectJobByIdAsync(jobId), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedJobDependencyException))), Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            int minutesInPast = GetRandomNegativeNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Job randomJob = CreateRandomJob(randomDateTime);
            Job someJob = randomJob;
            someJob.CreatedDate = randomDateTime.AddMinutes(minutesInPast);
            Guid jobId = someJob.Id;
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedJobException =
                new LockedJobException(databaseUpdateConcurrencyException);

            var expectedJobDependencyValidationException =
                new JobDependencyValidationException(lockedJobException);

            this.storageBrokerMock.Setup(broker =>
              broker.SelectJobByIdAsync(jobId))
                  .ThrowsAsync(databaseUpdateConcurrencyException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Returns(randomDateTime);

            // when
            ValueTask<Job> modifyJobTask =
                this.jobService.ModifyJobAsync(someJob);

            JobDependencyValidationException actualJobDependencyValidationException =
                await Assert.ThrowsAsync<JobDependencyValidationException>(modifyJobTask.AsTask);

            // then
            actualJobDependencyValidationException.Should()
                .BeEquivalentTo(expectedJobDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectJobByIdAsync(jobId), Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedJobDependencyValidationException))), Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            // given
            int minutesInPast = GetRandomNegativeNumber();
            var randomDateTime = GetRandomDateTime();
            Job randomJob = CreateRandomJob(randomDateTime);
            Job someJob = randomJob;
            someJob.CreatedDate = someJob.CreatedDate.AddMinutes(minutesInPast);
            var serviceException = new Exception();

            var failedJobServiceException =
                new FailedJobServiceException(serviceException);

            var expectedJobServiceException =
                new JobServiceException(failedJobServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectJobByIdAsync(someJob.Id))
                    .ThrowsAsync(serviceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTime);

            // when
            ValueTask<Job> modifyJobTask =
                this.jobService.ModifyJobAsync(someJob);

            JobServiceException actualJobServiceException =
                await Assert.ThrowsAsync<JobServiceException>(
                    modifyJobTask.AsTask);

            // then
            actualJobServiceException.Should().BeEquivalentTo(expectedJobServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectJobByIdAsync(someJob.Id), Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedJobServiceException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
