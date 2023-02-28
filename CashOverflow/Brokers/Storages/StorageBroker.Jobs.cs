// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System.Linq;
using CashOverflow.Models.Jobs;
using Microsoft.EntityFrameworkCore;

namespace CashOverflow.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Job> Jobs { get; set; }

        public IQueryable<Job> SelectAllJobs() =>
            SelectAll<Job>();
    }
}
