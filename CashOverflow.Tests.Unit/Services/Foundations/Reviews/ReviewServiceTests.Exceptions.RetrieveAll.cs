// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using CashOverflow.Models.Reviews.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace CashOverflow.Tests.Unit.Services.Foundations.Reviews
{
    public partial class ReviewServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = CreateSqlException();
            var failedReviewServiceException = new FailedReviewServiceException(sqlException);

            var expectedReviewDependencyException =
                new ReviewDependencyException(failedReviewServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllReviews()).Throws(sqlException);

            // when
            Action retrieveAllReviewAAction = () =>
                this.reviewService.RetrieveAllReviews();

            ReviewDependencyException actualReviewDependencyException =
                Assert.Throws<ReviewDependencyException>(retrieveAllReviewAAction);

            // then
            actualReviewDependencyException.Should().BeEquivalentTo(expectedReviewDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllReviews(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedReviewDependencyException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllWhenAllServicesErrorOccursAndLogIt()
        {
            // given
            string exceptionMessage = GetRandomMessage();
            var serviceException = new Exception(exceptionMessage);
            var failedReviewServiceException = new FailedReviewServiceException(serviceException);

            var expectedReviewServiceException =
                new ReviewServiceException(failedReviewServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllReviews()).Throws(serviceException);

            // when
            Action retrieveAllReviewAction = () =>
                this.reviewService.RetrieveAllReviews();

            ReviewServiceException actualReviewServiceException =
                Assert.Throws<ReviewServiceException>(retrieveAllReviewAction);

            // then
            actualReviewServiceException.Should().BeEquivalentTo(expectedReviewServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllReviews(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedReviewServiceException))),Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
