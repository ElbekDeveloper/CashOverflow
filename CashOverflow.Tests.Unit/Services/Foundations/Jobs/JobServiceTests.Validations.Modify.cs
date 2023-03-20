// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

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
        public async Task ShouldThrowValidationExceptionOnModifyIfJobIsNullAndLogItAsync()
        {
            // given
            Job nullJob = null;
            var nullJobException = new NullJobException();
            
            var expectedJobValidationException =
                new JobValidationException(nullJobException);

            // when
            ValueTask<Job> modifyJobTask = this.jobService.ModifyJobAsync(nullJob);

            JobValidationException actualJobValidationException =
                await Assert.ThrowsAsync<JobValidationException>(
                    modifyJobTask.AsTask);

            // then
            actualJobValidationException.Should().BeEquivalentTo(expectedJobValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedJobValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
