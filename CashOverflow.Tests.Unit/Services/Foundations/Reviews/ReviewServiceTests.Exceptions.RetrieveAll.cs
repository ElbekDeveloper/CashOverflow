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

            var failedStorageException = new FailedReviewStorageException(sqlException);

            var expectedReviewDependencyException = new ReviewDependencyException(failedStorageException);

            this.storageBrokerMock.Setup(broker => broker.SelectAllReviews()).Throws(sqlException);

            // when 
            Action retrieveAllReviewAction = () => this.reviewService.RetrieveAllReviews();

            ReviewDependencyException actualreviewDependencyException =
                Assert.Throws<ReviewDependencyException>(retrieveAllReviewAction);

            // then
            actualreviewDependencyException.Should().BeEquivalentTo(expectedReviewDependencyException);

            this.storageBrokerMock.Verify(broker => broker.SelectAllReviews(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogCritical(It.Is(SameExceptionAs(expectedReviewDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllWhenAllServiceErrorOccursAndLogIt()
        {
            // given 
            string exceptionMessage = GetRandomString();
            var serviceException = new Exception(exceptionMessage);

            var failedReviewServiceException = new FailedReviewServiceException(serviceException);

            var expectedReviewServiceException = new ReviewServiceException(failedReviewServiceException);

            this.storageBrokerMock.Setup(broker => broker.SelectAllReviews()).Throws(serviceException);

            // when 
            Action retrieveAllReviewAction = () => this.reviewService.RetrieveAllReviews();

            // then
            Assert.Throws<ReviewServiceException>(retrieveAllReviewAction);

            this.storageBrokerMock.Verify(broker => broker.SelectAllReviews(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(expectedReviewServiceException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
