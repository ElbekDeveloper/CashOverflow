// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Threading.Tasks;
using CashOverflow.Models.Jobs;
using Force.DeepCloner;
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
			Job expectedJob = persistedJob.DeepClone();

			// when
			Job actualJob = await this.jobService
				.AddJobAsync(inputJob);


			// then
		}
	}
}

