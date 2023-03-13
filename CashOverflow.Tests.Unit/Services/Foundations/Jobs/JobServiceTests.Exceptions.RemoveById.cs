// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashOverflow.Models.Jobs;
using CashOverflow.Models.Jobs.Exceptions;
using FluentAssertions;
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
    }
}
