using System;
using System.Threading.Tasks;
using CashOverflow.Brokers.Loggings;
using CashOverflow.Brokers.Storages;
using CashOverflow.Models.Jobs;

namespace CashOverflow.Services.Foundations.Jobs
{
    public partial class JobService : IJobService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;

        public JobService(IStorageBroker storageBroker, ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Job> RemoveJobByIdAsync(Guid jobId) =>
        TryCatch(async () =>
        {
            Job maybeJob = 
                await this.storageBroker.SelectJobByIdAsync(jobId);

            return await this.storageBroker.DeleteJobAsync(maybeJob);
        });
    }
}
