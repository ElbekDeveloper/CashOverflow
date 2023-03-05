using System;
using System.Threading.Tasks;
using CashOverflow.Models.Jobs;

namespace CashOverflow.Services.Foundations.Jobs
{
    public partial class JobService
    {
        private delegate ValueTask<Job> ReturningJobFunction();

        private async ValueTask<Job> TryCatch(ReturningJobFunction returningJobFunction)
        {
            try
            {
                return await returningJobFunction();
            }
            catch
            {
                throw new NotImplementedException();
            }
        }
    }
}
