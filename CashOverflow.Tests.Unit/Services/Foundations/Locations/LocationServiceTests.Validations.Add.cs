// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System.Threading.Tasks;
using CashOverflow.Models.Locations;
using CashOverflow.Models.Locations.Exceptions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Query;
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
			broker.LogError(It.Is(SameExceptionAs(expectedLocationValidationException))), Times.Once());

			this.storageBrokerMock.Verify(broker =>
			broker.InsertLocationAsync(It.IsAny <Location>()), Times.Never);

			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
        }

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData(" ")]

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
            key: nameof(Location.CreatedDate),
            values: "Date is required");

			var expectedLocationValidationException =
				new LocationValidationException(invalidLocationException);

			// when
			ValueTask<Location> AddLocationTask =
				this.locationService.AddLocationAsync(invalidLocation);

			LocationValidationException actualLocationValidationException =
				await Assert.ThrowsAsync<LocationValidationException>(AddLocationTask.AsTask);

			// then

			actualLocationValidationException.Should()
				.BeEquivalentTo(expectedLocationValidationException);
			loggingBrokerMock.Verify(broker =>
			 broker.LogError(It.Is(SameExceptionAs(
				actualLocationValidationException))), Times.Once);

			this.storageBrokerMock.Verify(broker =>
			broker.InsertLocationAsync(It.IsAny<Location>()), Times.Never);

			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
        }


    }
}

