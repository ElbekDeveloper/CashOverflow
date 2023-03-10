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
        public async Task ShouldRetrieveJobByIdAsync()
        {
            //given
            Guid randomJobId = Guid.NewGuid();
            Guid inputJobId = randomJobId;
            Job randomJob = CreateRandomJob();
            Job storageJob = randomJob;
            Job excpectedJob = randomJob.DeepClone();

            this.storageBrokerMock.Setup(broker =>
            broker.SelectJobByIdAsync(inputJobId)).ReturnsAsync(storageJob);

            //when
            Job actuallJob = await this.jobService.RetrieveJobByIdAsync(inputJobId);

            //then
            actuallJob.Should().BeEquivalentTo(excpectedJob);

            this.storageBrokerMock.Verify(broker =>
            broker.SelectJobByIdAsync(inputJobId), Times.Once());

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.timeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();

        }
    }
}
