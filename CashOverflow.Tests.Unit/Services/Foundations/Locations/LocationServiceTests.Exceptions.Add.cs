// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Threading.Tasks;
using CashOverflow.Models.Locations;
using CashOverflow.Models.Locations.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace CashOverflow.Tests.Unit.Services.Foundations.Locations
{
	public partial class LocationServiceTests
	{
		[Fact]
		public async Task ShouldThrowCriticalDependencyExceptionOnAddIfDependancyErrorOccursAndLogItAsync()
		{
			// given
			Location someLocation = CreateRandomLocation();
			SqlException sqlException = CreateSqlException();
			var failedLocationStorageException = new FailedLocationStorageException(sqlException);
			var expecteLocationDependancyException = new LocationDependancyException(failedLocationStorageException);

			this.dateTimeBrokerMock.Setup(broker =>
			broker.GetCurrentDateTimeOffset()).Throws(sqlException);

			// when
			ValueTask<Location> addLocationTask = this.locationService.AddLocationAsync(someLocation);

			LocationDependancyException actualLocationDependancyException =
				await Assert.ThrowsAsync<LocationDependancyException>(addLocationTask.AsTask);

            // then
            actualLocationDependancyException.Should().BeEquivalentTo(expecteLocationDependancyException);

			this.dateTimeBrokerMock.Verify(broker => broker.GetCurrentDateTimeOffset(), Times.Once);

			this.loggingBrokerMock.Verify(broker =>
			broker.LogCritical(It.Is(SameExceptionAs(expecteLocationDependancyException))), Times.Once);

			this.dateTimeBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}

