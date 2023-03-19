// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using CashOverflow.Models.Jobs;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CashOverflow.Services.Foundations.Jobs
{
    public interface IJobService
    {
        ValueTask<Job> AddJobAsync(Job job);
        IQueryable<Job> RetrieveAllJobs();
        ValueTask<Job> RetrieveJobByIdAsync(Guid jobId);
        ValueTask<Job> RemoveJobByIdAsync(Guid jobId);
    }
}

