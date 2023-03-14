// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
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
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public JobService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)

        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Job> RetrieveJobByIdAsync(Guid jobId) =>
           TryCatch(async () =>
           {
               ValidateJobId(jobId);

               Job maybeJob =
                   await storageBroker.SelectJobByIdAsync(jobId);

               ValidateStorageJobExists(maybeJob, jobId);

               return maybeJob;
           });

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
