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
		public async Task ShouldAddJobAsync()
		{
			// given
			DateTimeOffset randomDateTime = GetRandomDatetimeOffset();
			Job randomJob = CreateRandomJob(randomDateTime);
			Job inputJob = randomJob;
			Job persistedJob = inputJob;
			Job expectedJob = persistedJob.DeepClone();

			this.dateTimeBrokerMock.Setup(broker => broker.GetCurrentDateTimeOffset())
				.Returns(randomDateTime);

			this.storageBrokerMock.Setup(broker =>
			broker.InsertJobAsync(inputJob)).ReturnsAsync(persistedJob);

			// when
			Job actualJob = await this.jobService.AddJobAsync(inputJob);

			// then
			actualJob.Should().BeEquivalentTo(expectedJob);
			this.dateTimeBrokerMock.Verify(broker => broker.GetCurrentDateTimeOffset(), Times.Once);
            this.storageBrokerMock.Verify(broker =>broker.InsertJobAsync(inputJob), Times.Once);
			this.storageBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
		}
	}
}

