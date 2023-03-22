// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Location
// --------------------------------------------------------

using System;
using System.Threading.Tasks;
using CashOverflow.Models.Locations;
using CashOverflow.Models.Locations.Exceptions;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Xunit;

namespace CashOverflow.Tests.Unit.Services.Foundations.Locations
{
    public partial class LocationServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfLocationIsNullAndLogItAsync()
        {
            // given
            Location nullLocation = null;
            var nullLocationException = new NullLocationException();

            var expectedLocationValidationException =
                new LocationValidationException(nullLocationException);

            // when
            ValueTask<Location> modifyLocationTask =
                this.locationService.ModifyLocationAsync(nullLocation);

            LocationValidationException actualLocationValidationException =
                await Assert.ThrowsAsync<LocationValidationException>(
                    modifyLocationTask.AsTask);

            // then
            actualLocationValidationException.Should().BeEquivalentTo(expectedLocationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLocationValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateLocationAsync(It.IsAny<Location>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfLocationIsInvalidAndLogItAsync(string invalidString)
        {
            // given 
            var invalidLocation = new Location
            {
                Name = invalidString
            };

            var invalidLocationException =
                new InvalidLocationException();

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
                "Date is required",
                "Date is not recent",
                $"Date is the same as {nameof(Location.CreatedDate)}");


            var expectedLocationValidationException =
                new LocationValidationException(invalidLocationException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(GetRandomDateTime);

            // when
            ValueTask<Location> modifyLocationTask =
                this.locationService.ModifyLocationAsync(invalidLocation);

            LocationValidationException actualLocationValidationException =
                await Assert.ThrowsAsync<LocationValidationException>(
                    modifyLocationTask.AsTask);

            //then
            actualLocationValidationException.Should()
                .BeEquivalentTo(expectedLocationValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLocationValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateLocationAsync(It.IsAny<Location>()), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }


        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsNotSameAsCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Location randomLocation = CreateRandomLocation(randomDateTime);
            Location invalidLocation = randomLocation;
            var invalidLocationException = new InvalidLocationException();

            invalidLocationException.AddData(
                key: nameof(Location.UpdatedDate),
                values: $"Date is the same as {nameof(Location.CreatedDate)}");

            var expectedLocationValidationException =
                new LocationValidationException(invalidLocationException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Returns(randomDateTime);

            // when
            ValueTask<Location> modifyLocationTask =
                this.locationService.ModifyLocationAsync(invalidLocation);

            LocationValidationException actualLocationValidationException =
                await Assert.ThrowsAsync<LocationValidationException>(
                    modifyLocationTask.AsTask);

            // then
            actualLocationValidationException.Should()
                .BeEquivalentTo(expectedLocationValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLocationValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLocationByIdAsync(invalidLocation.Id), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidSeconds))]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsNotRecentAndLogItAsync(int seconds)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Location randomLocation = CreateRandomLocation(dateTime);
            Location inputLocation = randomLocation;
            inputLocation.UpdatedDate = dateTime.AddMinutes(seconds);
            var invalidLocationException = new InvalidLocationException();

            invalidLocationException.AddData(
                key: nameof(Location.UpdatedDate),
                values: "Date is not recent");

            var expectedLocationValidatonException =
                new LocationValidationException(invalidLocationException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Returns(dateTime);

            // when
            ValueTask<Location> modifyLocationTask =
                this.locationService.ModifyLocationAsync(inputLocation);

            LocationValidationException actualLocationValidationException =
                await Assert.ThrowsAsync<LocationValidationException>(
                    modifyLocationTask.AsTask);

            // then
            actualLocationValidationException.Should().BeEquivalentTo(expectedLocationValidatonException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLocationValidatonException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLocationByIdAsync(It.IsAny<Guid>()), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfLocationDoesNotExistAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetRandomNegativeNumber();
            DateTimeOffset dateTime = GetRandomDateTime();
            Location randomLocation = CreateRandomLocation(dateTime);
            Location nonExistLocation = randomLocation;
            nonExistLocation.CreatedDate = dateTime.AddMinutes(randomNegativeMinutes);
            Location nullLocation = null;

            var notFoundLocationException =
                new NotFoundLocationException(nonExistLocation.Id);

            var expectedLocationValidationException =
                new LocationValidationException(notFoundLocationException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectLocationByIdAsync(nonExistLocation.Id)).ReturnsAsync(nullLocation);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Returns(dateTime);

            // when 
            ValueTask<Location> modifyLocationTask =
                this.locationService.ModifyLocationAsync(nonExistLocation);

            LocationValidationException actualLocationValidationException =
               await Assert.ThrowsAsync<LocationValidationException>(modifyLocationTask.AsTask);

            // then
            actualLocationValidationException.Should().BeEquivalentTo(expectedLocationValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLocationByIdAsync(nonExistLocation.Id), Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLocationValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedDateNotSameAsCreatedDateAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNegativeNumber();
            int randomMinutes = randomNumber;
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Location randomLocation = CreateRandomModifyLocation(randomDateTime);
            Location invalidLocation = randomLocation.DeepClone();
            Location storageLocation = invalidLocation.DeepClone();
            storageLocation.CreatedDate = storageLocation.CreatedDate.AddMinutes(randomMinutes);
            storageLocation.UpdatedDate = storageLocation.UpdatedDate.AddMinutes(randomMinutes);
            var invalidLocationException = new InvalidLocationException();
            Guid LocationId = invalidLocation.Id;

            invalidLocationException.AddData(
                key: nameof(Location.CreatedDate),
                values: $"Date is not same as {nameof(Location.CreatedDate)}.");

            var expectedLocationValidationException =
                new LocationValidationException(invalidLocationException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectLocationByIdAsync(LocationId)).ReturnsAsync(storageLocation);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Returns(randomDateTime);

            // when
            ValueTask<Location> modifyLocationTask =
                this.locationService.ModifyLocationAsync(invalidLocation);

            LocationValidationException actualLocationValidationException =
                await Assert.ThrowsAsync<LocationValidationException>(modifyLocationTask.AsTask);

            // then
            actualLocationValidationException.Should().BeEquivalentTo(expectedLocationValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLocationByIdAsync(invalidLocation.Id), Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedLocationValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
