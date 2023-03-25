// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using Moq;
using System.Threading.Tasks;
using Xunit;
using CashOverflow.Models.Reviews;
using FluentAssertions;
using Force.DeepCloner;

namespace CashOverflow.Tests.Unit.Services.Foundations.Reviews
{
	public partial class ReviewServiceTests
	{
        [Fact]
        public async Task ShouldAddReviewAsync()
        {
            // given
            DateTimeOffset randomDate = GetRandomDateTimeOffset();
            Review randomReview = CreateRandomReview(randomDate);
            Review inputReview = randomReview;
            Review persistedReview = inputReview;
            Review expectedReview = persistedReview.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
            broker.GetCurrentDateTimeOffset()).Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertReviewAsync(inputReview)).ReturnsAsync(persistedReview);

            // when
            Review actualReview = await this.reviewService
                .AddReviewAsync(inputReview);

            // then
            actualReview.Should().BeEquivalentTo(expectedReview);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertReviewAsync(inputReview), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}

