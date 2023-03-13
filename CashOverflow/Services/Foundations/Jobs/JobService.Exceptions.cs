// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Threading.Tasks;
using CashOverflow.Models.Jobs;
using CashOverflow.Models.Jobs.Exceptions;
using Microsoft.AspNetCore.Authentication;
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
			catch (NullJobException nullJobException)
            {
              throw  CreateAndLogValidationException(nullJobException);
            }
            catch(InvalidJobException invalidJobException)
            {
                throw CreateAndLogValidationException(invalidJobException);
            }
        }

        private JobValidationException CreateAndLogValidationException(Xeption exception)
        {
            var jobValidationException = new JobValidationException(exception);
            this.loggingBroker.LogError(jobValidationException);

            return jobValidationException;
        }

    }
}

