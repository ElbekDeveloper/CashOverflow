// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Threading.Tasks;
using CashOverflow.Models.Locations;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Xunit;

namespace CashOverflow.Tests.Unit.Services.Foundations.Locations
{
    public partial class LocationServiceTests
    {
        [Fact]
        public async Task ShouldModifyLocationAsync()
        {
            //given
            DateTimeOffset randomDate = GetRandomDatetimeOffset();
            Location randomLocation = CreateRandomModifyLocation(randomDate);
            Location inputLocation = randomLocation;
            Location storageLocation = inputLocation.DeepClone();
            storageLocation.UpdatedDate = randomLocation.CreatedDate;
            Location updatedLocation = inputLocation;
            Location expectedLocation = updatedLocation.DeepClone();
            Guid locationId = inputLocation.Id;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectLocationByIdAsync(locationId))
                    .ReturnsAsync(storageLocation);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateLocationAsync(inputLocation))
                    .ReturnsAsync(updatedLocation);

            //when
            Location actualLocation = await 
                this.locationService.ModifyLocationAsync(inputLocation);

            //then
            actualLocation.Should().BeEquivalentTo(expectedLocation);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLocationByIdAsync(locationId), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateLocationAsync(inputLocation), Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();

        }
    }
}
