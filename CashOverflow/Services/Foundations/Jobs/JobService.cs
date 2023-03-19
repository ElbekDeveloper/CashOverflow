// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using CashOverflow.Brokers.DateTimes;
using CashOverflow.Brokers.Loggings;
using CashOverflow.Brokers.Storages;
using CashOverflow.Models.Jobs;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xeptions;

namespace CashOverflow.Services.Foundations.Jobs
{
    public partial class JobService : IJobService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public JobService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        private Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        public ValueTask<Job> AddJobAsync(Job job) =>
            TryCatch(async () =>
        {
            ValidateJobOnAdd(job);

            return await this.storageBroker.InsertJobAsync(job);
        });

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
