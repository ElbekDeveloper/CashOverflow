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
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            //given
            var invalidJobId = Guid.Empty;
            var invalidJobException = new InvalidJobException();

            invalidJobException.AddData(
                key: nameof(Job.Id),
                values: "Id is required");

            var excpectedJobValidationException = new
                JobValidationException(invalidJobException);

            //when
            ValueTask<Job> retrieveJobByIdTask =
                this.jobService.RetrieveJobByIdAsync(invalidJobId);

            JobValidationException actuallJobValidationException =
                await Assert.ThrowsAsync<JobValidationException>(
                    retrieveJobByIdTask.AsTask);

            //then
            actuallJobValidationException.Should().BeEquivalentTo(excpectedJobValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                excpectedJobValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectJobByIdAsync(It.IsAny<Guid>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRetrieveByIdIfTeamIsNotFoundAndLogItAsync()
        {
            Guid someJobId = Guid.NewGuid();
            Job noJob = null;

            var notFoundJobException =
                new NotFoundJobException(someJobId);

            var excpectedJobValidationException =
                new JobValidationException(notFoundJobException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectJobByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(noJob);

            //when 
            ValueTask<Job> retrieveJobByIdTask =
                this.jobService.RetrieveJobByIdAsync(someJobId);

            JobValidationException actualJobValidationException =
                await Assert.ThrowsAsync<JobValidationException>(
                        retrieveJobByIdTask.AsTask);

            //then
            actualJobValidationException.Should().BeEquivalentTo(excpectedJobValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectJobByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    excpectedJobValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
