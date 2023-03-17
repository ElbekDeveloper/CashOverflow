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
using Moq;
using Xunit;

namespace CashOverflow.Tests.Unit.Services.Foundations.Locations
{
    public partial class LocationServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyValidationOnRemoveIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Guid someLocationId = Guid.NewGuid();

            var databaseUpdateConcurrencyException =
                new DbUpdateConcurrencyException();

            var lockedLocationException =
                new LockedLocationException(databaseUpdateConcurrencyException);

            var expectedLocationDependencyValidationException =
                new LocationDependencyValidationException(lockedLocationException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectLocationByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Location> removeLocationByIdTask =
                this.locationService.RemoveLocationByIdAsync(someLocationId);

            LocationDependencyValidationException actualLocationDependencyValidationException =
                await Assert.ThrowsAsync<LocationDependencyValidationException>(
                    removeLocationByIdTask.AsTask);

            // then
            actualLocationDependencyValidationException.Should().BeEquivalentTo(
                expectedLocationDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLocationByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLocationDependencyValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteLocationAsync(It.IsAny<Location>()), Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someLocationId = Guid.NewGuid();
            SqlException sqlException = CreateSqlException();

            var failedLocationStorageException =
                new FailedLocationStorageException(sqlException);

            var expectedLocationDependencyException =
                new LocationDependencyException(failedLocationStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectLocationByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);
            // when
            ValueTask<Location> deleteLocationTask =
                this.locationService.RemoveLocationByIdAsync(someLocationId);

            LocationDependencyException actualLocationDependencyException =
                await Assert.ThrowsAsync<LocationDependencyException>(
                    deleteLocationTask.AsTask);

            // then
            actualLocationDependencyException.Should().BeEquivalentTo(
                expectedLocationDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLocationByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedLocationDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveIfExceptionOccursAndLogItAsync()
        {
            // given
            Guid someLocationId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedLocationServiceException =
                new FailedLocationServiceException(serviceException);

            var expectedLocationServiceException =
                new LocationServiceException(failedLocationServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectLocationByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Location> removeLocationByIdTask =
                this.locationService.RemoveLocationByIdAsync(someLocationId);

            LocationServiceException actualLocationServiceException =
                await Assert.ThrowsAsync<LocationServiceException>(
                    removeLocationByIdTask.AsTask);

            // then
            actualLocationServiceException.Should().BeEquivalentTo(
                expectedLocationServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLocationByIdAsync(It.IsAny<Guid>()),
                        Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLocationServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
