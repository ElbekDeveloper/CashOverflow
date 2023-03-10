// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using CashOverflow.Brokers.Storages;
using CashOverflow.Models.Jobs;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CashOverflow.Tests.Unit.Services.Foundations.Jobs
{
    public partial class JobServiceTests
    {
        [Fact]
        public async Task ShouldAddJobAsync() 
        {
            // given
            Job randomJob = CreateRandomJob();
            Job inputJob = randomJob;
            Job persistedJob = inputJob;
            Job expectedJob=persistedJob.DeepClone();

            this.storageBrokerMock.Setup(broker =>
            broker.InsertJobAsync(inputJob)).ReturnsAsync(persistedJob);
            // when
            Job actualJob = await this.jobService.AddJobAsync(inputJob);
            
            // then
            actualJob.Should().BeEquivalentTo(expectedJob);

            this.storageBrokerMock.Verify(broker => broker.InsertJobAsync(inputJob), Times.Once());
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
