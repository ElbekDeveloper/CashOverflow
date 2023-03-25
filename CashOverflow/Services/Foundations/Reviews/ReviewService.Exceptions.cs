// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using CashOverflow.Models.Reviews;
using CashOverflow.Models.Reviews.Exceptions;
using Microsoft.Data.SqlClient;
using Xeptions;

namespace CashOverflow.Services.Foundations.Reviews
{
    public partial class ReviewService
    {
        private delegate ValueTask<Review> ReturningReviewFunction();
        private delegate IQueryable<Review> ReturningReviewsFunction();

        private IQueryable<Review> TryCatch(ReturningReviewsFunction
            returningReviewFunction)
        {
            try
            {
                return returningReviewFunction();
            }
            catch (SqlException sqlException)
            {
                var failedReviewServiceException =
                    new FailedReviewServiceException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedReviewServiceException);
            }
            catch (Exception exception)
            {
                var failedReviewServiceException = new FailedReviewServiceException(exception);

                throw CreateAndLogServiceException(failedReviewServiceException);
            }
        }

        private ReviewServiceException CreateAndLogServiceException(Xeption exception)
        {
            var reviewServiceException = new ReviewServiceException(exception);
            this.loggingBroker.LogError(reviewServiceException);

            return reviewServiceException;
        }

        private ReviewDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var reviewDependencyException = new ReviewDependencyException(exception);
            this.loggingBroker.LogCritical(reviewDependencyException);

            return reviewDependencyException;
        }
    }
}
