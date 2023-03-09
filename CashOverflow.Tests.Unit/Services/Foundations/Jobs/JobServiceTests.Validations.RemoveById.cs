// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Threading.Tasks;
using CashOverflow.Models.Jobs;
using CashOverflow.Models.Jobs.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace CashOverflow.Tests.Unit.Services.Foundations.Jobs
{
    public partial class JobServiceTests
    {
        [Fact]
        private async Task ShouldThrowValidationExceptionOnRemoveIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidJobId = Guid.Empty;

            var invalidJobException = new InvalidJobException();

            invalidJobException.AddData(
                key: nameof(Job.Id),
                values: "Id is required");

            var expectedJobValidationException =
                new JobValidationException(invalidJobException);

            // when
            ValueTask<Job> removeJobByIdTask = 
                this.jobService.RemoveJobByIdAsync(invalidJobId);

            JobValidationException actualJobValidationException =
                await Assert.ThrowsAsync<JobValidationException>(
                    removeJobByIdTask.AsTask);

            // then
            actualJobValidationException.Should().BeEquivalentTo(actualJobValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedJobValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteJobAsync(It.IsAny<Job>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowNotFoundExceptionOnRemoveIfTeamIsNotFoundAndLogItAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputJobId = randomId;
            Job noJob = null;

            var notFoundJobException =
                new NotFoundJobException(inputJobId);

            var expectedJobValidationException =
                new JobValidationException(notFoundJobException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectJobByIdAsync(inputJobId)).ReturnsAsync(noJob);

            // when
            ValueTask<Job> removeJobByIdAsync =
                this.jobService.RemoveJobByIdAsync(inputJobId);

            JobValidationException actualJobValidationException =
                await Assert.ThrowsAsync<JobValidationException>(
                    removeJobByIdAsync.AsTask);

            // then
            actualJobValidationException.Should().BeEquivalentTo(expectedJobValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectJobByIdAsync(inputJobId), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedJobValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();

        }
    }
}
