// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.IO.Compression;
using System.Threading.Tasks;
using CashOverflow.Models.Locations;
using CashOverflow.Models.Locations.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace CashOverflow.Tests.Unit.Services.Foundations.Locations
{
    public partial class LocationServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfInputIsNullAndLogItAsync()
        {
            // given
            Location nullLocation = null;
            var nullLocationException = new NullLocationException();

            var expectedLocationValidationException =
                new LocationValidationException(nullLocationException);

            // when
            ValueTask<Location> addLocationTask = this.locationService.AddLocationAsync(nullLocation);

            LocationValidationException actualLocationValidationException =
                await Assert.ThrowsAsync<LocationValidationException>(addLocationTask.AsTask);

            // then
            actualLocationValidationException.Should().BeEquivalentTo(expectedLocationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedLocationValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertLocationAsync(It.IsAny<Location>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnAddIfLocationIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            var invalidLocation = new Location
            {
                Name = invalidText
            };

            var invalidLocationException = new InvalidLocationException();

            invalidLocationException.AddData(
                key: nameof(Location.Id),
                values: "Id is required");

            invalidLocationException.AddData(
                key: nameof(Location.Name),
                values: "Text is required");

            invalidLocationException.AddData(
                key: nameof(Location.CreatedDate),
                values: "Date is required");

            invalidLocationException.AddData(
                key: nameof(Location.UpdatedDate),
                values: "Date is required");

            var expectedLocationValidationException =
                new LocationValidationException(invalidLocationException);

            // when
            ValueTask<Location> addLocationTask =
                this.locationService.AddLocationAsync(invalidLocation);

            LocationValidationException actualLocationValidationException =
                await Assert.ThrowsAsync<LocationValidationException>(addLocationTask.AsTask);

            // then
            actualLocationValidationException.Should()
                .BeEquivalentTo(expectedLocationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLocationValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertLocationAsync(It.IsAny<Location>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreatedDateIsNotSameAsUpdatedDateAndLogItAsync()
        {
            // given
            int randomMinutes = GetRandomNumber();
            DateTimeOffset randomDate = GetRandomDatetimeOffset();
            Location randomLocation = CreateRandomLocation(randomDate);
            Location invalidLocation = randomLocation;
            invalidLocation.UpdatedDate = randomDate.AddMinutes(randomMinutes);
            var invalidLocationException = new InvalidLocationException();

            invalidLocationException.AddData(
                key: nameof(Location.CreatedDate),
                values: $"Date is not same as {nameof(Location.UpdatedDate)}");

            var expectedLocationValidationException = new LocationValidationException(invalidLocationException);

            // when
            ValueTask<Location> addLocationTask = this.locationService.AddLocationAsync(invalidLocation);

            LocationValidationException actualLocationValidationException =
                await Assert.ThrowsAsync<LocationValidationException>(addLocationTask.AsTask);

            // then
            actualLocationValidationException.Should().BeEquivalentTo(expectedLocationValidationException);

            this.loggingBrokerMock.Verify(broker => broker.LogError(It.Is(SameExceptionAs(
                expectedLocationValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker => broker.InsertLocationAsync(It.IsAny<Location>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidMinutes))]
        public async Task ShouldThrowValidationExceptionOnAddIfCreatedDateIsNotRecentAndLogItAsync(
            int invalidMinutes)
        {
            // given
            DateTimeOffset randomDate = GetRandomDatetimeOffset();
            DateTimeOffset invalidDateTime = randomDate.AddMinutes(invalidMinutes);
            Location randomLocation = CreateRandomLocation(invalidDateTime);
            Location invalidLocation = randomLocation;
            var invalidLocationException = new InvalidLocationException();

            invalidLocationException.AddData(
                key: nameof(Location.CreatedDate),
                values: "Date is not recent");

            var expectedLocationValidationException = new LocationValidationException(invalidLocationException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Returns(randomDate);

            // when
            ValueTask<Location> addLocationTask = this.locationService.AddLocationAsync(invalidLocation);

            LocationValidationException actualLocationValidationException =
                await Assert.ThrowsAsync<LocationValidationException>(addLocationTask.AsTask);

            // then
            actualLocationValidationException.Should().BeEquivalentTo(expectedLocationValidationException);

            this.dateTimeBrokerMock.Verify(broker => broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker => broker.LogError(It.Is(
                SameExceptionAs(expectedLocationValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker => broker.InsertLocationAsync(It.IsAny<Location>()), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
