// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System.Linq;
using CashOverflow.Models.Reviews;

namespace CashOverflow.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        IQueryable<Review> SelectAllReviews();
    }
}