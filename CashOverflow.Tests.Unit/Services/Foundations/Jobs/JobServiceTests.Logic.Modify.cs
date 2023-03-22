// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Threading.Tasks;
using CashOverflow.Models.Jobs;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Xunit;

namespace CashOverflow.Tests.Unit.Services.Foundations.Jobs
{
    public partial class JobServiceTests
    {
        [Fact]
        public async Task ShouldModifyJobAsync()
        {
            // given
            DateTimeOffset randomDate = GetRandomDateTime();
            Job randomJob = CreateRandomModifyJob(randomDate);
            Job inputJob = randomJob;
            Job storageJob = inputJob.DeepClone();
            storageJob.UpdatedDate = randomJob.CreatedDate;
            Job updatedJob = inputJob;
            Job expectedJob = updatedJob.DeepClone();
            Guid jobId = inputJob.Id;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectJobByIdAsync(jobId))
                    .ReturnsAsync(storageJob);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateJobAsync(inputJob))
                    .ReturnsAsync(updatedJob);

            // when
            Job actualJob =
               await this.jobService.ModifyJobAsync(inputJob);

            // then
            actualJob.Should().BeEquivalentTo(expectedJob);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectJobByIdAsync(jobId), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateJobAsync(inputJob), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
