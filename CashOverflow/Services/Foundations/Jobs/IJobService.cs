using System;
using System.Threading.Tasks;
using CashOverflow.Models.Jobs;

namespace CashOverflow.Services.Foundations.Jobs
{
    public interface IJobService
    {
        ValueTask<Job> RemoveJobByIdAsync(Guid jobId);
    }
}
