// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Linq;
using CashOverflow.Models.Salaries;
using CashOverflow.Models.Salaries.Exceptions;
using Microsoft.Data.SqlClient;
using Xeptions;

namespace CashOverflow.Services.Foundations.Salaries
{
    public partial class SalaryService
    {
        private delegate IQueryable<Salary> ReturningSalaryFunction();

        private IQueryable<Salary> TryCatch(ReturningSalaryFunction returningSalaryFunction)
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

            this.loggingBroker.LogError(salaryDependencyException);

            throw salaryDependencyException;
        }
    }
}
