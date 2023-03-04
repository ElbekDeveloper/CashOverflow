using System;
using System.Threading.Tasks;
using CashOverflow.Brokers.Loggings;
using CashOverflow.Brokers.Storages;
using CashOverflow.Models.Jobs;

namespace CashOverflow.Services.Foundations.Jobs
{
    public class JobService : IJobService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;

        public JobService(IStorageBroker storageBroker, ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Job> RemoveJobByIdAsync(Guid jobId) =>
            throw new NotImplementedException();
    }
}
