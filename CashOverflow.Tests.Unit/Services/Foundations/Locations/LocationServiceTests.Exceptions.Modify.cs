// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

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
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            //given
            Location someLocation = CreateRandomLocation();
            SqlException sqlException = CreateSqlException();

            var failedLocationStorageException =
                new FailedLocationStorageException(sqlException);

            var expectedLocationDependencyException =
                new LocationDependencyException(failedLocationStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            //when
            ValueTask<Location> modifyLocationTask =
                this.locationService.ModifyLocationAsync(someLocation);

            LocationDependencyException actualLocationDependencyException =
                await Assert.ThrowsAsync<LocationDependencyException>(
                    modifyLocationTask.AsTask);

            //then
            actualLocationDependencyException.Should()
                .BeEquivalentTo(expectedLocationDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedLocationDependencyException))),Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateLocationAsync(It.IsAny<Location>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLocationByIdAsync(someLocation.Id),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
