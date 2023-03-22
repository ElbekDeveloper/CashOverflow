// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Location
// --------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;
using CashOverflow.Models.Locations;
using CashOverflow.Models.Locations.Exceptions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace CashOverflow.Tests.Unit.Services.Foundations.Locations
{
    public partial class LocationServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            DateTimeOffset someDateTime = GetRandomDateTime();
            Location randomLocation = CreateRandomLocation(someDateTime);
            Location someLocation = randomLocation;
            Guid LocationId = someLocation.Id;
            SqlException sqlException = CreateSqlException();

            var failedLocationStorageException =
                new FailedLocationStorageException(sqlException);

            var expectedLocationDependencyException =
                new LocationDependencyException(failedLocationStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Throws(sqlException);

            // when
            ValueTask<Location> modifyLocationTask =
                this.locationService.ModifyLocationAsync(someLocation);

            LocationDependencyException actualLocationDependencyException =
                await Assert.ThrowsAsync<LocationDependencyException>(
                     modifyLocationTask.AsTask);

            // then
            actualLocationDependencyException.Should().BeEquivalentTo(
                expectedLocationDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedLocationDependencyException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLocationByIdAsync(LocationId), Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateLocationAsync(someLocation), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            int minutesInPast = GetRandomNegativeNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Location randomLocation = CreateRandomLocation(randomDateTime);
            Location someLocation = randomLocation;
            Guid LocationId = someLocation.Id;
            someLocation.CreatedDate = randomDateTime.AddMinutes(minutesInPast);
            var databaseUpdateException = new DbUpdateException();

            var failedLocationException =
                new FailedLocationStorageException(databaseUpdateException);

            var expectedLocationDependencyException =
                new LocationDependencyException(failedLocationException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectLocationByIdAsync(LocationId))
                    .ThrowsAsync(databaseUpdateException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Returns(randomDateTime);

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
                broker.SelectLocationByIdAsync(LocationId), Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLocationDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            int minutesInPast = GetRandomNegativeNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Location randomLocation = CreateRandomLocation(randomDateTime);
            Location someLocation = randomLocation;
            someLocation.CreatedDate = randomDateTime.AddMinutes(minutesInPast);
            Guid LocationId = someLocation.Id;
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedLocationException =
                new LockedLocationException(databaseUpdateConcurrencyException);

            var expectedLocationDependencyValidationException =
                new LocationDependencyValidationException(lockedLocationException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectLocationByIdAsync(LocationId))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Returns(randomDateTime);

            // when
            ValueTask<Location> modifyLocationTask =
                this.locationService.ModifyLocationAsync(someLocation);

            LocationDependencyValidationException actualLocationDependencyValidationException =
                await Assert.ThrowsAsync<LocationDependencyValidationException>(
                    modifyLocationTask.AsTask);

            // then
            actualLocationDependencyValidationException.Should().BeEquivalentTo(
                expectedLocationDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLocationByIdAsync(LocationId), Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLocationDependencyValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            // given
            int minuteInPast = GetRandomNegativeNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Location randomLocation = CreateRandomLocation(randomDateTime);
            Location someLocation = randomLocation;
            someLocation.CreatedDate = randomDateTime.AddMinutes(minuteInPast);
            var serviceException = new Exception();

            var failedLocationException =
                new FailedLocationServiceException(serviceException);

            var expectedLocationServiceException =
                new LocationServiceException(failedLocationException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectLocationByIdAsync(someLocation.Id))
                    .ThrowsAsync(serviceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Returns(randomDateTime);

            // when
            ValueTask<Location> modifyLocationTask =
                this.locationService.ModifyLocationAsync(someLocation);

            LocationServiceException actualLocationServiceException =
                await Assert.ThrowsAsync<LocationServiceException>(
                    modifyLocationTask.AsTask);

            // then
            actualLocationServiceException.Should().BeEquivalentTo(
                expectedLocationServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLocationByIdAsync(someLocation.Id), Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLocationServiceException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
