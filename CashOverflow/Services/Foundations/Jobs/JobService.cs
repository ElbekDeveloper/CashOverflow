using CashOverflow.Brokers.Storages;
using CashOverflow.Models.Jobs;
using System.Threading.Tasks;

namespace CashOverflow.Services.Foundations.Jobs
{
    public class JobService : IJobService
    {
        private readonly IStorageBroker storageBroker;

        public JobService(IStorageBroker storageBroker)
        {
            this.storageBroker = storageBroker;

        }

        public async ValueTask<Job> AddJobAsync(Job job)
        {

            return await this.storageBroker.InsertJobAsync(job);
        }
    }
}
