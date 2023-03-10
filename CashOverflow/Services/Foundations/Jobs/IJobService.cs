using CashOverflow.Models.Jobs;
using System.Threading.Tasks;

namespace CashOverflow.Services.Foundations.Jobs
{
    public interface IJobService
    {
        ValueTask<Job> AddJobAsync(Job job); 
    }
}
