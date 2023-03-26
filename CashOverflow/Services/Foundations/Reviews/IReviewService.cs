// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using CashOverflow.Models.Locations;
using CashOverflow.Models.Reviews;

namespace CashOverflow.Services.Foundations.Reviews
{
    public interface IReviewService
    {
        ValueTask<Review> AddReviewAsync(Review review);
        IQueryable<Review> RetrieveAllReviews();
    }
}
