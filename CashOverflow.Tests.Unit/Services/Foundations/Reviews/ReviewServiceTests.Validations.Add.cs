// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Threading.Tasks;
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

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}

