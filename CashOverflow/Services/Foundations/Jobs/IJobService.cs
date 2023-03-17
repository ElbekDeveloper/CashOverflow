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
        ValueTask<Job> RetrieveJobByIdAsync(Guid jobId);
        IQueryable<Job> RetrieveAllJobs();
        ValueTask<Job> RemoveJobByIdAsync(Guid jobId);
    }
}
