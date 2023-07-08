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
        /// <exception cref="Models.Jobs.Exceptions.JobValidationException"></exception>
        /// <exception cref="Models.Jobs.Exceptions.JobDependencyValidationException"></exception>
        /// <exception cref="Models.Jobs.Exceptions.JobDependencyException"></exception>
        /// <exception cref="Models.Jobs.Exceptions.JobServiceException"></exception>
        ValueTask<Job> AddJobAsync(Job job);

        /// <exception cref="Models.Jobs.Exceptions.JobDependencyException"></exception>
        /// <exception cref="Models.Jobs.Exceptions.JobServiceException"></exception>
        IQueryable<Job> RetrieveAllJobs();

        /// <exception cref="Models.Jobs.Exceptions.JobDependencyException"></exception>
        /// <exception cref="Models.Jobs.Exceptions.JobServiceException"></exception>
        ValueTask<Job> RetrieveJobByIdAsync(Guid jobId);

        /// <exception cref="Models.Jobs.Exceptions.JobValidationException"></exception>
        /// <exception cref="Models.Jobs.Exceptions.JobDependencyValidationException"></exception>
        /// <exception cref="Models.Jobs.Exceptions.JobDependencyException"></exception>
        /// <exception cref="Models.Jobs.Exceptions.JobServiceException"></exception>
        ValueTask<Job> ModifyJobAsync(Job job);

        /// <exception cref="Models.Jobs.Exceptions.JobDependencyValidationException"></exception>
        /// <exception cref="Models.Jobs.Exceptions.JobDependencyException"></exception>
        /// <exception cref="Models.Jobs.Exceptions.JobServiceException"></exception>
        ValueTask<Job> RemoveJobByIdAsync(Guid jobId);
    }
}
