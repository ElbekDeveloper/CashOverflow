// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using CashOverflow.Models.Salaries;
using CashOverflow.Models.Salaries.Exceptions;
using Microsoft.Data.SqlClient;
using Xeptions;

namespace CashOverflow.Services.Foundations.Salaries
{
    public partial class SalaryService
    {
        private delegate ValueTask<Salary> ReturningSalaryFunction();
        private delegate IQueryable<Salary> ReturningSalariesFunction();

        private async ValueTask<Salary> TryCatch(ReturningSalaryFunction returningSalaryFunction)
        {
            try
            {
                return await returningSalaryFunction();
            }
            catch (NullSalaryException nullSalaryException)
            {
                throw CreateAndLogValidationException(nullSalaryException);
            }
        }

        private IQueryable<Salary> TryCatch(ReturningSalariesFunction returningSalaryFunction)
        {
            try
            {
                return returningSalaryFunction();
            }
            catch (SqlException sqlException)
            {
                var failedSalaryStorageException = new FailedSalaryStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedSalaryStorageException);
            }
            catch (Exception exception)
            {
                var failedSalaryServiceException = new FailedSalaryServiceException(exception);

                throw CreateAndLogServiceException(failedSalaryServiceException);
            }
        }

        private SalaryValidationException CreateAndLogValidationException(Xeption exception)
        {
            var salaryValidationException = new SalaryValidationException(exception);
            this.loggingBroker.LogError(salaryValidationException);

            return salaryValidationException;
        }

        private SalaryServiceException CreateAndLogServiceException(Xeption exception)
        {
            var createAndLogServiceException =
                new SalaryServiceException(exception);

            this.loggingBroker.LogError(createAndLogServiceException);

            throw createAndLogServiceException;
        }

        private SalaryDependencyException CreateAndLogCriticalDependencyException(Xeption xeption)
        {
            var salaryDependencyException =
                new SalaryDependencyException(xeption);

            this.loggingBroker.LogCritical(salaryDependencyException);

            throw salaryDependencyException;
        }
    }
}
