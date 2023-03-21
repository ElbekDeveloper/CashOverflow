// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using CashOverflow.Models.Jobs;

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
