// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Threading.Tasks;
using CashOverflow.Models.Companies;
using CashOverflow.Models.Companies.Exceptions;
using EFxceptions.Models.Exceptions;
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
            catch (NotFoundCompanyException notFoundCompanyException)
            {
                throw CreateAndLogValidationException(notFoundCompanyException);
            }
            catch (SqlException sqlException)
            {
                var failedCompanyStorageException = new FailedCompanyStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedCompanyStorageException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedCompanyException = new LockedCompanyException(dbUpdateConcurrencyException);

                throw CreateAndLogCriticalDependencyException(lockedCompanyException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                var failedCompanyStorageException = new FailedCompanyStorageException(dbUpdateException);

                throw CreateAndLogCriticalDependencyException(failedCompanyStorageException);
            }
            catch (Exception exception)
            {
                var failedCompanyServiceException = new FailedCompanyServiceException(exception);

                throw CreateAndLogServiceException(failedCompanyServiceException);
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
        
        private Exception CreateAndLogServiceException(FailedCompanyServiceException innerException)
        {
            var companyServiceException = new CompanyServiceException(innerException);
            this.loggingBroker.LogError(companyServiceException);

            return companyServiceException;
        }
    }
}