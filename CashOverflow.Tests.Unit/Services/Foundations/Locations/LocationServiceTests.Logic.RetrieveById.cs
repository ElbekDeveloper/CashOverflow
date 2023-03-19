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
        public async Task ShouldRetrieveJobByIdAsync()
        {
            //given
            Guid randomLocationId = Guid.NewGuid();
            Guid inputLocationId = randomLocationId;
            Location randomLocation = CreateRandomLocation();
            Location storageLocation = randomLocation;
            Location excpectedLocation = randomLocation.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectLocationByIdAsync(inputLocationId)).ReturnsAsync(storageLocation);

            //when
            Location actuallLocation = await this.locationService.RetrieveLocationByIdAsync(inputLocationId);

            //then
            actuallLocation.Should().BeEquivalentTo(excpectedLocation);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLocationByIdAsync(inputLocationId), Times.Once());

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
