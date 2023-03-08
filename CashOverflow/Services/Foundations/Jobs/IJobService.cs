using System;
using System.Threading.Tasks;
using CashOverflow.Models.Jobs;

namespace CashOverflow.Services.Foundations.Jobs
{
    public partial interface IJobService
    {
        ValueTask<Job> RemoveJobByIdAsync(Guid JobId);
    }
}
