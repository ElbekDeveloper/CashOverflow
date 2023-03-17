﻿// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
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
            catch(Exception exception)
            {
                FailedSalaryServiceException faildedSalaction = new FailedSalaryServiceException(exception);    

                throw CreateAndLogServiceException(faildedSalaction);
            }
        }

        private SalaryServiceException CreateAndLogServiceException(Xeption exception)
        {
               var salaryServiceException = new SalaryServiceException(exception);
            this.loggingBroker.LogError(salaryServiceException);

            return salaryServiceException;
        }

        private SalaryValidationException CreateAndLogValidationException(Xeption exception)
        {
            var salaryValidationException = new SalaryValidationException(exception);
            this.loggingBroker.LogError(salaryValidationException);

            return salaryValidationException;
        }

        private SalaryDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var salaryDependencyException = new SalaryDependencyException(exception);
            this.loggingBroker.LogCritical(salaryDependencyException);

            return salaryDependencyException;
        }

        private SalaryDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var salaryDependencyValidationException = new SalaryDependencyValidationException(exception);
            this.loggingBroker.LogError(salaryDependencyValidationException);

            return salaryDependencyValidationException;
        }
    }
}
