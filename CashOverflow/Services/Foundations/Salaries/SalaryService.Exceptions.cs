﻿// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using CashOverflow.Models.Salaries;
using CashOverflow.Models.Salaries.Exceptions;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Xeptions;

namespace CashOverflow.Services.Foundations.Salaries
{
    public partial class SalaryService
    {
        private delegate ValueTask<Salary> ReturningSalaryFucnction();
        private delegate IQueryable<Salary> ReturningSalaryFunction();

        private async ValueTask<Salary> TryCatch(ReturningSalaryFucnction returningSalaryFunction)
        {
            try
            {
                return await returningSalaryFunction();
            }
            catch (NullSalaryException nullSalaryException)
            {
                throw CreateAndLogValidationException(nullSalaryException);
            }
            catch (InvalidSalaryException invalidSalaryException)
            {
                throw CreateAndLogValidationException(invalidSalaryException);
            }
            catch (SqlException sqlException)
            {
                var failedSalaryStorageException = new FailedSalaryStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedSalaryStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsSalaryException = new AlreadyExistsSalaryException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsSalaryException);
            }
        }

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

        private SalaryDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var salaryDependencyValidationException = new SalaryDependencyValidationException(exception);
            this.loggingBroker.LogError(salaryDependencyValidationException);

            return salaryDependencyValidationException;
        }
    }
}
