// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CashOverflow.Brokers.DateTimes;
using CashOverflow.Brokers.Loggings;
using CashOverflow.Brokers.Storages;
using CashOverflow.Models.Jobs;

namespace CashOverflow.Services.Foundations.Jobs
{
    public partial class JobService : IJobService
    {

        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public JobService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)

        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public IQueryable<Job> RetrieveAllJobs() =>
            TryCatch(() => this.storageBroker.SelectAllJobs());

        public ValueTask<Job> RetrieveJobByIdAsync(Guid jobId) =>
           TryCatch(async () =>
           {
               ValidateJobId(jobId);

               Job maybeJob =
                   await storageBroker.SelectJobByIdAsync(jobId);

               ValidateStorageJobExists(maybeJob, jobId);

               return maybeJob;
           });

        public async ValueTask<Job> ModifyJobAsync(Job job)
        {
            Job maybeJob = 
                await this.storageBroker.SelectJobByIdAsync(job.Id);
            
            var currentDate = this.dateTimeBroker.GetCurrentDateTimeOffset();
           
            return  await this.storageBroker.UpdateJobAsync(job);
        }

        public ValueTask<Job> RemoveJobByIdAsync(Guid jobId) =>
           TryCatch(async () =>
           {
               ValidateJobId(jobId);

               Job maybeJob =
                   await this.storageBroker.SelectJobByIdAsync(jobId);

               ValidateStorageJobExists(maybeJob, jobId);

               return await this.storageBroker.DeleteJobAsync(maybeJob);
           });

    }
}
