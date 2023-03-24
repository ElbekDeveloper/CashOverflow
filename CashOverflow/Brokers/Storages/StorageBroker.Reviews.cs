// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using CashOverflow.Models.Reviews;
using Microsoft.EntityFrameworkCore;

namespace CashOverflow.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Review> Reviews { get; set; }

        public async ValueTask<Review> InsertReviewAsync(Review review) =>
            await InsertAsync(review);

        public IQueryable<Review> SelectAllReviews() => SelectAll<Review>();
    }
}
