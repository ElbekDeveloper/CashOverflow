//using System.Data.SqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CashOverflow.Models.Jobs;
using CashOverflow.Models.Jobs.Exceptions;
using Xeptions;
using System.Runtime.Serialization;
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
        }
        private JobDependencyException CreatedAndLogCriticalDependencyException(Xeption exception)
        {
            var jobDependencyException = new JobDependencyException(exception);
            this.loggingBroker.LogCritical(jobDependencyException);

            return jobDependencyException;
        }
    }
}