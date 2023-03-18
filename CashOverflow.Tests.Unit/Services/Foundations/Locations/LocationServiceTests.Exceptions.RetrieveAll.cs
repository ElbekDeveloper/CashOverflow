// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
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
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = CreateSqlException();

            var failedStorageException =
                new FailedLocationStorageException(sqlException);

            var expectedLocationDependencyException =
                new LocationDependencyException(failedStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllLocations())
                    .Throws(sqlException);

            // when
            Action retrieveAllLocationsAction = () =>
               this.locationService.RetrieveAllLocations();

            LocationDependencyException actualLocationDependencyException =
                Assert.Throws<LocationDependencyException>(
                    retrieveAllLocationsAction);

            // then
            actualLocationDependencyException.Should().BeEquivalentTo(
               expectedLocationDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllLocations(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedLocationDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}