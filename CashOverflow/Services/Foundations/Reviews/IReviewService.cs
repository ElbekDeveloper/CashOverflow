// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using CashOverflow.Models.Reviews;

namespace CashOverflow.Services.Foundations.Reviews
{
    public interface IReviewService
    {
        /// <exception cref="Models.Reviews.Exceptions.ReviewValidationException"></exception>
        /// <exception cref="Models.Reviews.Exceptions.ReviewDependencyValidationException"></exception>
        /// <exception cref="Models.Reviews.Exceptions.ReviewDependencyException"></exception>
        /// <exception cref="Models.Reviews.Exceptions.ReviewServiceException"></exception>
        ValueTask<Review> AddReviewAsync(Review review);

        /// <exception cref="Models.Reviews.Exceptions.ReviewDependencyException"></exception>
        /// <exception cref="Models.Reviews.Exceptions.ReviewServiceException"></exception>
        IQueryable<Review> RetrieveAllReviews();
    }
}
