// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Linq;
using CashOverflow.Models.Reviews;
using CashOverflow.Models.Reviews.Exceptions;
using Microsoft.Data.SqlClient;
using Xeptions;

namespace CashOverflow.Services.Foundations.Reviews
{
    public partial class ReviewService
    {
        private delegate IQueryable<Review> ReturningReviewsFunctions();

        private IQueryable<Review> TryCatch(ReturningReviewsFunctions returningReviewsFunctions)
        {
            try
            {
                return returningReviewsFunctions();
            }
            catch (SqlException sqlException)
            {
                var failedReviewStorageException = new FailedReviewStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedReviewStorageException);
            }
            catch (Exception serviceException)
            {
                var failedReviewServiceException = new FailedReviewServiceException(serviceException);

                throw CreateAndLogServiceException(failedReviewServiceException);
            }
        }

        private ReviewServiceException CreateAndLogServiceException(Xeption exception)
        {
            var reviewServiceException = new ReviewServiceException(exception);
            this.loggingBroker.LogError(reviewServiceException);

            return reviewServiceException;
        }

        private ReviewDependencyException CreateAndLogCriticalDependencyException(Xeption xeption)
        {
            var reviewDependencyException = new ReviewDependencyException(xeption);

            this.loggingBroker.LogCritical(reviewDependencyException);

            throw reviewDependencyException;
        }
    }
}
