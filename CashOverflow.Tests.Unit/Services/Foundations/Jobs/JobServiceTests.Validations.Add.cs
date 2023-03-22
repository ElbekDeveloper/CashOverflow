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
		public async Task ShouldThrowValidationExceptionOnAddIfInputIsNullAndLogItAsync()
		{
			// given
			Job nullJob = null;
			var nullJobException = new NullJobException();

			var expectedJobValidationException =
				new JobValidationException(nullJobException);

			// when
			ValueTask<Job> addJobTask = this.jobService.AddJobAsync(nullJob);

			JobValidationException actualJobValidationException =
				await Assert.ThrowsAsync<JobValidationException>(addJobTask.AsTask);

			// then
			actualJobValidationException.Should()
				.BeEquivalentTo(expectedJobValidationException);

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(
					SameExceptionAs(expectedJobValidationException))), Times.Once);

			this.storageBrokerMock.Verify(broker =>
			broker.InsertJobAsync(It.IsAny<Job>()), Times.Never);

			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
		}
	}
}

