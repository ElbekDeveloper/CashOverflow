// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using CashOverflow.Models.Reviews;
using CashOverflow.Models.Reviews.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using EFxceptions.Models.Exceptions;

namespace CashOverflow.Tests.Unit.Services.Foundations.Reviews
{
    public partial class ReviewServiceTests
    {

        [Fact]
        public async Task ShoudlThrowCriticalDependencyExceptionOnAddIfDependencyErrorOccursAndLogItAsync()
        {
            // given

            Review someReview = CreateRandomReview(GetRandomStarsInRange());
            SqlException sqlException = CreateSqlException();
            var failedReviewStorageException = new FailedReviewStorageException(sqlException);

            var expectedReviewDependencyException =
                new ReviewDependencyException(failedReviewStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertReviewAsync(someReview)).
                    ThrowsAsync(sqlException);

            // when
            ValueTask<Review> addReviewTask = this.reviewService.AddReviewAsync(someReview);

            ReviewDependencyException actualReviewDependencyException =
                await Assert.ThrowsAsync<ReviewDependencyException>(addReviewTask.AsTask);

            // then
            actualReviewDependencyException.Should()
                .BeEquivalentTo(expectedReviewDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(
                    SameExceptionAs(expectedReviewDependencyException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
            broker.InsertReviewAsync(someReview), Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}

