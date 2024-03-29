﻿// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System.Linq;
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
        public void ShouldRetrieveAllJobs()
        {
            //given
            IQueryable<Job> randomJobs = CreateRandomJobs();
            IQueryable<Job> storageJobs = randomJobs;
            IQueryable<Job> expectedJobs = storageJobs.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllJobs()).Returns(storageJobs);

            //when
            IQueryable<Job> actualJobs =
                this.jobService.RetrieveAllJobs();

            //then
            actualJobs.Should().BeEquivalentTo(expectedJobs);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllJobs(), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
