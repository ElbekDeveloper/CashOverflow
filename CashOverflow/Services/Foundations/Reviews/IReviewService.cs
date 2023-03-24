// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System.Linq;
using CashOverflow.Models.Reviews;

namespace CashOverflow.Services.Foundations.Reviews
{
    public interface IReviewService
    {
        IQueryable<Review> RetrieveAllReviews();
    }
}
