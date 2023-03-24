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
        private delegate IQueryable<Review> ReturningReviewFunction();

        private IQueryable<Review> TryCatch(ReturningReviewFunction
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
        }

        private ReviewDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var reviewDependencyException = new ReviewDependencyException(exception);
            this.loggingBroker.LogCritical(reviewDependencyException);

            return reviewDependencyException;
        }
    }
}
