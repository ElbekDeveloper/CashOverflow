using System;
using System.Threading.Tasks;
using System.Windows.Markup;
using CashOverflow.Models.Jobs;
using CashOverflow.Models.Jobs.Exceptions;
using FluentAssertions;
using Force.DeepCloner;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
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
    }
}
