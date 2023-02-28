// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System.Linq;
using CashOverflow.Models.Jobs;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CashOverflow.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Job> Jobs { get; set; }

        public async ValueTask<Job> InsertJobAsync(Job job) =>
            await InsertAsync(job);

        public IQueryable<Job> SelectAllJobs() =>
            SelectAll<Job>();
    }
}
