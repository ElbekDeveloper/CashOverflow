// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using CashOverflow.Models.Jobs;

namespace CashOverflow.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Job> InsertJobAsync(Job job);
        IQueryable<Job> SelectAllJobs();
        ValueTask<Job> SelectJobByIdAsync(Guid jobId);
        ValueTask<Job> DeleteJobAsync(Job job);
    }
}
