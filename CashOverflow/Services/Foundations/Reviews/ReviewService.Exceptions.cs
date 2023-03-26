// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using CashOverflow.Models.Reviews;
using CashOverflow.Models.Reviews.Exceptions;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Xeptions;

namespace CashOverflow.Services.Foundations.Reviews
{
    public partial class ReviewService
    {
        private delegate ValueTask<Review> ReturningReviewFunction();
        private delegate IQueryable<Review> ReturningReviewsFunction();
        private async ValueTask<Review> TryCatch(ReturningReviewFunction returningReviewFunction)
        {
            try
            {
                return await returningReviewFunction();
            }
            catch (NullReviewException nullReviewException)
            {
                throw CreateAndLogValidationException(nullReviewException);
            }
            catch (InvalidReviewException invalidReviewException)
            {
                throw CreateAndLogValidationException(invalidReviewException);
            }
            catch (SqlException sqlException)
            {
                var failedReviewStorageException =
                    new FailedReviewStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedReviewStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsReviewException =
                    new AlreadyExistsReviewException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsReviewException);
            }
        }

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

        private ReviewValidationException CreateAndLogValidationException(Xeption exception)
        {
            var reviewValidationException = new ReviewValidationException(exception);
            this.loggingBroker.LogError(reviewValidationException);

            return reviewValidationException;
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

        private ReviewDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var reviewDependencyValidationException =
                new ReviewDependencyValidationException(exception);

            this.loggingBroker.LogError(reviewDependencyValidationException);

            return reviewDependencyValidationException;
        }
    }
}
