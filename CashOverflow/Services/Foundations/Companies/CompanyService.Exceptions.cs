// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Threading.Tasks;
using CashOverflow.Models.Companies;
using CashOverflow.Models.Companies.Exceptions;
using EFxceptions.Models.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace CashOverflow.Services.Foundations.Companies
{
    public partial class CompanyService
    {
        private delegate ValueTask<Company> ReturningCompanyFunction();

        private async ValueTask<Company> TryCatch(ReturningCompanyFunction returningCompanyFunction)
        {
            try
            {
                return await returningCompanyFunction();
            }
            catch (NullCompanyException nullCompanyException)
            {
                throw CreateAndLogValidationException(nullCompanyException);
            }
            catch (InvalidCompanyException invalidCompanyException)
            {
                throw CreateAndLogValidationException(invalidCompanyException);
            }
            catch (SqlException sqlException)
            {
                var failedCompanyStorageException = new FailedCompanyStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedCompanyStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsCompanyException =
                    new AlreadyExistsCompanyException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsCompanyException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedCompanyException = new LockedCompanyException(dbUpdateConcurrencyException);
                
                throw CreateAndLogErrorDependencyException(lockedCompanyException);
            }
        }

        private CompanyValidationException CreateAndLogValidationException(Xeption exception)
        {
            var companyValidationException = new CompanyValidationException(exception);
            this.loggingBroker.LogError(companyValidationException);

            return companyValidationException;
        }

        private CompanyDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var companyDependencyException = new CompanyDependencyException(exception);
            this.loggingBroker.LogCritical(companyDependencyException);

            return companyDependencyException;
        }

        private Exception CreateAndLogDependencyValidationException(Xeption exception)
        {
            var companyDependencyValidationException =
                new CompanyDependencyValidationException(exception);

            this.loggingBroker.LogError(companyDependencyValidationException);

            return companyDependencyValidationException;
        }
        
        private CompanyDependencyException CreateAndLogErrorDependencyException(Xeption exception)
        {
            var companyDependencyException = new CompanyDependencyException(exception);
            this.loggingBroker.LogError(companyDependencyException);

            return companyDependencyException;
        }

    }
}
