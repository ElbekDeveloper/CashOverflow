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
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
        {
            //given
            Guid someId = Guid.NewGuid();
            SqlException sqlException = CreateSqlException();

            var failedLocationException =
                new FailedLocationStorageException(sqlException);

            var excpectedLocationDependencyException =
                new LocationDependencyException(failedLocationException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectLocationByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            //when
            ValueTask<Location> retrieveLocationById =
                this.locationService.RetrieveLocationByIdAsync(someId);

            LocationDependencyException actuallLocationDependencyException =
                await Assert.ThrowsAnyAsync<LocationDependencyException>(
                    retrieveLocationById.AsTask);

            //then
            actuallLocationDependencyException.Should()
                    .BeEquivalentTo(excpectedLocationDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLocationByIdAsync(It.IsAny<Guid>()));

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    excpectedLocationDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdAsyncIfServiceErrorOccursAndLogItAsync()
        {
            //given
            Guid someId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedLocationException =
                new FailedLocationServiceException(serviceException);

            var excpectedLocationServiceException =
                new LocationServiceException(failedLocationException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectLocationByIdAsync(It.IsAny<Guid>())).ThrowsAsync(serviceException);

            //when
            ValueTask<Location> retrieveLocationById =
                this.locationService.RetrieveLocationByIdAsync(someId);

            LocationServiceException actuallLocationServiceException =
                await Assert.ThrowsAsync<LocationServiceException>(
                    retrieveLocationById.AsTask);

            //then
            actuallLocationServiceException.Should().BeEquivalentTo(excpectedLocationServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLocationByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    excpectedLocationServiceException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
