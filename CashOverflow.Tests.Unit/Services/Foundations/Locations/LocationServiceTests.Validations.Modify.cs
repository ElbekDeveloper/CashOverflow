﻿// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

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
            //given
            Location nullLocation = null;
            var nullLocationException = new NullLocationException();

            var expectedLocationValidationException =
                new LocationValidationException(nullLocationException);

            //when
            ValueTask<Location> modifyLocationTask =
                this.locationService.ModifyLocationAsync(nullLocation);

            //then
            await Assert.ThrowsAsync<LocationValidationException>(() =>
                modifyLocationTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedLocationValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateLocationAsync(It.IsAny<Location>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfLocationIsInvalidAndLogItAsync(string invalidText)
        {
            //given
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
                values: "Value is required");
            
            invalidLocationException.AddData(
                key: nameof(Location.UpdatedDate),
                values: "Value is required");

            var expectedLocationValidationException =
                new LocationValidationException(invalidLocationException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Returns(GetRandomDatetimeOffset);

            //when
            ValueTask<Location> modifyLocationTask =
                this.locationService.ModifyLocationAsync(invalidLocation);

            LocationValidationException actualLocationValidationException =
                await Assert.ThrowsAsync<LocationValidationException>(modifyLocationTask.AsTask);

            //then
            actualLocationValidationException.Should().BeEquivalentTo(
                expectedLocationValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedLocationValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateLocationAsync(It.IsAny<Location>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
