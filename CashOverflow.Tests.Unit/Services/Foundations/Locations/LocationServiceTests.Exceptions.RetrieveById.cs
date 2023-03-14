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
    }
}
