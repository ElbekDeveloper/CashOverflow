// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System.Threading.Tasks;
using CashOverflow.Models.Locations;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Xunit;

namespace CashOverflow.Tests.Unit.Services.Foundations.Locations {
    public partial class LocationServiceTests {
        [Fact]
        public async Task ShouldAddLocationAsync() {
            // given
            Location randomLocation = CreateRandomLocation();
            Location inputLocation = randomLocation;
            Location persistedLocation = inputLocation;
            Location expectedLocation = persistedLocation.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.InsertLocationAsync(inputLocation)).ReturnsAsync(persistedLocation);

            // when
            Location actualLocation = await this.locationService
                .AddLocationAsync(inputLocation);

            // then
            actualLocation.Should().BeEquivalentTo(expectedLocation);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertLocationAsync(inputLocation), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
