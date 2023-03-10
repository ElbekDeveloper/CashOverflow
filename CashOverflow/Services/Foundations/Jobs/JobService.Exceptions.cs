// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System.Threading.Tasks;
using CashOverflow.Models.Jobs;
using CashOverflow.Models.Jobs.Exceptions;
using Xeptions;

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
            catch (InvalidJobException inalidJobException)
            {
                throw CreateAndLogValidationException(inalidJobException);
            }
        }

        private JobValidationException CreateAndLogValidationException(Xeption exception)
        {
            var jobValidationExpcetion = new JobValidationException(exception);
            this.loggingBroker.LogError(jobValidationExpcetion);

            return jobValidationExpcetion;
        }
    }
}

    