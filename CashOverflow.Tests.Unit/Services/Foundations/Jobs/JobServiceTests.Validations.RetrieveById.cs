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
            this.timeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
