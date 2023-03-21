// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Threading.Tasks;
using CashOverflow.Models.Jobs;
using CashOverflow.Models.Jobs.Exceptions;
using Castle.Components.DictionaryAdapter.Xml;
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
                new JobDependencyException(databaseUpdateException);

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

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedJobDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();

        }
    }
}
