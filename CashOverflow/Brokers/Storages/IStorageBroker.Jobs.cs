// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Threading.Tasks;
using CashOverflow.Models.Jobs;

namespace CashOverflow.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Job> InsertJobAsync(Job job);
        ValueTask<Job> SelectJobByIdAsync(Guid jobId);
        IQueryable<Job> SelectAllJobs();
    }
}
