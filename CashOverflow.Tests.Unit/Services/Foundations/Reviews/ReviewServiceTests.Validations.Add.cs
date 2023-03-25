// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Threading.Tasks;
using CashOverflow.Models.Reviews;
using CashOverflow.Models.Reviews.Exceptions;
using CashOverflow.Models.Reviews;
using CashOverflow.Models.Reviews.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace CashOverflow.Tests.Unit.Services.Foundations.Reviews
{
    public partial class ReviewServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfInputIsNullAndLogItAsync()
        {
            // given
            Review nullReview = null;
            var nullReviewException = new NullReviewException();

            var expectedReviewValidationException =
                new ReviewValidationException(nullReviewException);

            // when 
            ValueTask<Review> addReviewTask =
                this.reviewService.AddReviewAsync(nullReview);

            ReviewValidationException actualReviewValidationException =
                await Assert.ThrowsAsync<ReviewValidationException>(addReviewTask.AsTask);

            // then
            actualReviewValidationException.Should()
                .BeEquivalentTo(expectedReviewValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedReviewValidationException)))
                    , Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertReviewAsync(It.IsAny<Review>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfReviewIsInvalidAndLogItAsync(
    string invalidText)
        {
            // given
            Review invalidReview = new Review()
            {
                CompanyName = invalidText,
                Thoughts = invalidText
            };

            var invalidReviewException = new InvalidReviewException();

            invalidReviewException.AddData(
                key: nameof(Review.Id),
                values: "Id is required");

            invalidReviewException.AddData(
                key: nameof(Review.CompanyName),
                values: "Company name is required");

            invalidReviewException.AddData(
                key: nameof(Review.Stars),
                values: "Stars are required");

            invalidReviewException.AddData(
                key: nameof(Review.Thoughts),
                values: "Thoughts are required");

            var expectedReviewValidationException =
                new ReviewValidationException(invalidReviewException);

            // when
            ValueTask<Review> addReviewTask = this.reviewService.AddReviewAsync(invalidReview);

            ReviewValidationException actualReviewValidationException =
                await Assert.ThrowsAsync<ReviewValidationException>(addReviewTask.AsTask);

            // then
            actualReviewValidationException.Should()
                .BeEquivalentTo(expectedReviewValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedReviewValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertReviewAsync(It.IsAny<Review>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}

