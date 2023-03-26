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
using Microsoft.EntityFrameworkCore;
using CashOverflow.Models.Reviews;
using CashOverflow.Models.Reviews.Exceptions;

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

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfDublicateKeyErrorOccursAndLogItAsync()
        {
            // given
            string someMessage = GetRandomString();
            Review someReview = CreateRandomReview(GetRandomStarsInRange());
            var duplicateKeyException = new DuplicateKeyException(someMessage);
            var alreadyExistsReviewException = new AlreadyExistsReviewException(duplicateKeyException);

            var expectedReviewDependencyValidationException =
                new ReviewDependencyValidationException(alreadyExistsReviewException);

            this.storageBrokerMock.Setup(broker => broker.InsertReviewAsync(someReview))
                .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<Review> addReviewTask = this.reviewService.AddReviewAsync(someReview);

            ReviewDependencyValidationException actualReviewDependencyValidationException =
                await Assert.ThrowsAsync<ReviewDependencyValidationException>(addReviewTask.AsTask);

            // then
            actualReviewDependencyValidationException.Should()
                .BeEquivalentTo(expectedReviewDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertReviewAsync(someReview), Times.Once);

            this.loggingBrokerMock.Verify(broker => broker.LogError(It.Is(
                SameExceptionAs(expectedReviewDependencyValidationException))), Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Review someReview = CreateRandomReview(GetRandomStarsInRange());
            var serviceException = new Exception();
            var failedReviewServiceException = new FailedReviewServiceException(serviceException);

            var expectedReviewServiceException =
                new ReviewServiceException(failedReviewServiceException);

            this.storageBrokerMock.Setup(broker => broker.InsertReviewAsync(someReview))
                .Throws(serviceException);

            // when
            ValueTask<Review> addReviewTask = this.reviewService.AddReviewAsync(someReview);

            ReviewServiceException actualReviewServiceException =
                await Assert.ThrowsAsync<ReviewServiceException>(addReviewTask.AsTask);

            // then
            actualReviewServiceException.Should().BeEquivalentTo(expectedReviewServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertReviewAsync(someReview), Times.Once);

            this.loggingBrokerMock.Verify(broker => broker.LogError(It.Is(SameExceptionAs(
                expectedReviewServiceException))), Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}

