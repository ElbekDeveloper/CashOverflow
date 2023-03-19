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
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
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
            DateTimeOffset someDateTime = GetRandomDateTime();
            Location randomLocation = CreateRandomLocation(someDateTime);
            Location someLocation = randomLocation;
            Guid locationId = someLocation.Id;
            SqlException sqlException = CreateSqlException();

            var failedLocationException =
                new FailedLocationStorageException(sqlException);

            var expectedLocationDependencyException =
                new LocationDependencyException(failedLocationException);

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
            actualLocationDependencyException.Should().BeEquivalentTo(
                expectedLocationDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectJobByIdAsync(locationId), Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedLocationDependencyException))),Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLocationByIdAsync(locationId), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            int minutesInPast = GetRandomNegativeNumber();
            DateTimeOffset randomDateTimeOffSet = GetRandomDatetimeOffset();
            Location randomLocation = CreateRandomLocation(randomDateTimeOffSet);
            Location someLocation = randomLocation;
            Guid locationId = someLocation.Id;
            someLocation.CreatedDate = randomDateTimeOffSet.AddMinutes(minutesInPast);
            var databaseUpdateException = new DbUpdateException();

            var failedLocationException =
                new FailedLocationStorageException(databaseUpdateException);

            var expectedLocationDependencyException =
                new LocationDependencyException(failedLocationException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectLocationByIdAsync(locationId))
                    .ThrowsAsync(databaseUpdateException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Returns(randomDateTimeOffSet);

            // when
            ValueTask<Location> modifyLocationTask =
                this.locationService.ModifyLocationAsync(someLocation);

            LocationDependencyException actualLocationDependencyException =
                 await Assert.ThrowsAsync<LocationDependencyException>(
                     modifyLocationTask.AsTask);

            // then
            actualLocationDependencyException.Should().BeEquivalentTo(
                expectedLocationDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLocationByIdAsync(locationId), Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedLocationDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
