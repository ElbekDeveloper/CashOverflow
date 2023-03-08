// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Linq;
using CashOverflow.Models.Jobs;
using CashOverflow.Models.Jobs.Exceptions;
using Xeptions;
using Microsoft.Data.SqlClient;

namespace CashOverflow.Services.Foundations.Jobs
{
    public partial class JobService
    {
        private delegate IQueryable<Job> ReturningJobsFunction();
        private IQueryable<Job> TryCatch(ReturningJobsFunction returningJobsFunction)
        {
            try
            {
                return returningJobsFunction();
            }
            catch(SqlException sqlException)
            {
                var failedJobStorageException = new FailedJobStorageException(sqlException);
                
                throw CreatedAndLogCriticalDependencyException(failedJobStorageException);
            }
            catch(Exception exception)
            {
                var failedJobServiceException =
                    new FailedJobServiceException(exception);
                
                throw CreateAndLogServiceException(failedJobServiceException);
            }
        }
        private JobDependencyException CreatedAndLogCriticalDependencyException(Xeption exception)
        {
            var jobDependencyException = new JobDependencyException(exception);
            this.loggingBroker.LogCritical(jobDependencyException);

            return jobDependencyException;
        }
        private JobServiceException CreateAndLogServiceException(Xeption exception)
        {
            var JobServiceException = new JobServiceException(exception);

            this.loggingBroker.LogError(JobServiceException);

            return JobServiceException;
        }
    }
}