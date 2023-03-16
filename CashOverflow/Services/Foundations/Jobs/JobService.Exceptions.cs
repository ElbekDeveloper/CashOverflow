// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Threading.Tasks;
using CashOverflow.Models.Jobs;
using CashOverflow.Models.Jobs.Exceptions;
using Microsoft.Data.SqlClient;
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
            catch (NotFoundJobException notFoundJobException)
            {
                throw CreateAndLogValidationException(notFoundJobException);
            }
            catch (SqlException sqlException)
            {
                var failedJobStorageException = new FailedJobStorageException(sqlException);

                throw CreateAndLogDependencyException(failedJobStorageException);
            }
            catch (Exception exception)
            {
                var failedJobServiceException = new FailedJobServiceException(exception);

                throw CreateAndLogServiceException(failedJobServiceException);
            }
        }

        private JobValidationException CreateAndLogValidationException(Xeption exception)
        {
            var jobValidationExpcetion = new JobValidationException(exception);
            this.loggingBroker.LogError(jobValidationExpcetion);

            return jobValidationExpcetion;
        }

        private JobDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var jobDependencyException = new JobDependencyException(exception);
            this.loggingBroker.LogCritical(jobDependencyException);

            return jobDependencyException;
        }

        private JobServiceException CreateAndLogServiceException(Xeption innerException)
        {
            var jobServiceException = new JobServiceException(innerException);
            this.loggingBroker.LogError(jobServiceException);

            return jobServiceException;
        }
    }
}

