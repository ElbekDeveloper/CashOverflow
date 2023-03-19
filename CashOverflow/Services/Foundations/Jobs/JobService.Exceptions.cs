// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using CashOverflow.Models.Jobs;
using CashOverflow.Models.Jobs.Exceptions;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xeptions;

namespace CashOverflow.Services.Foundations.Jobs
{
    public partial class JobService
    {
        private delegate ValueTask<Job> ReturningJobFunction();
        private delegate IQueryable<Job> ReturningJobsFunction();

        private async ValueTask<Job> TryCatch(ReturningJobFunction returningJobFunction)
        {
            try
            {
                return await returningJobFunction();
            }
            catch (NullJobException nullJobException)
            {
                throw CreateAndLogValidationException(nullJobException);
            }
            catch (InvalidJobException invalidJobException)
            {
                throw CreateAndLogValidationException(invalidJobException);
            }
            catch (NotFoundJobException notFoundJobException)
            {
                throw CreateAndLogValidationException(notFoundJobException);
            }
            catch (SqlException sqlException)
            {
                var failedJobStorageException = new FailedJobStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedJobStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsJobException = new AlreadyExistsJobException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsJobException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedJobException = new LockedJobException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(lockedJobException);
            }
            catch (Exception exception)
            {
                var failedJobServiceException = new FailedJobServiceException(exception);

                throw CreateAndLogServiceException(failedJobServiceException);
            }
        }

        private IQueryable<Job> TryCatch(ReturningJobsFunction returningJobsFunction)
        {
            try
            {
                return returningJobsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedJobStorageException = new FailedJobStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedJobStorageException);
            }
            catch (Exception serviceException)
            {
                var failedJobServiceException = new FailedJobServiceException(serviceException);

                throw CreateAndLogServiceException(failedJobServiceException);
            }
        }

        private JobValidationException CreateAndLogValidationException(Xeption exception)
        {
            var jobValidationException = new JobValidationException(exception);
            this.loggingBroker.LogError(jobValidationException);

            return jobValidationException;
        }

        private JobDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var jobDependencyException = new JobDependencyException(exception);
            this.loggingBroker.LogCritical(jobDependencyException);

            return jobDependencyException;
        }

        private JobDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var jobDependencyException = new JobDependencyException(exception);
            this.loggingBroker.LogCritical(jobDependencyException);

            return jobDependencyException;
        }

        private JobDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var jobDependencyValidationException = new JobDependencyValidationException(exception);
            this.loggingBroker.LogError(jobDependencyValidationException);

            return jobDependencyValidationException;
        }

        private JobServiceException CreateAndLogServiceException(Xeption exception)
        {
            var jobServiceException = new JobServiceException(exception);
            this.loggingBroker.LogError(jobServiceException);

            return jobServiceException;
        }
    }
}

