// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using CashOverflow.Models.Languages;
using CashOverflow.Models.Languages.Exceptions;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace CashOverflow.Services.Foundations.Languages
{
    public partial class LanguageService
    {
        private delegate ValueTask<Language> ReturningLanguageFunction();
        private delegate IQueryable<Language> ReturningLanguagesFunction();

        private async ValueTask<Language> TryCatch(ReturningLanguageFunction returningLanguageFunction)
        {
            try
            {
                return await returningLanguageFunction();
            }
            catch (NullLanguageException nullLanguageException)
            {
                throw CreateAndLogValidationException(nullLanguageException);
            }
            catch (InvalidLanguageException invalidLanguageException)
            {
                throw CreateAndLogValidationException(invalidLanguageException);
            }
            catch (NotFoundLanguageException notFoundLanguageException)
            {
                throw CreateAndLogValidationException(notFoundLanguageException);
            }
            catch (SqlException sqlException)
            {
                var failedLanguageStorageException = new FailedLanguageStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedLanguageStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsLanguageException = new AlreadyExistsLanguageException(duplicateKeyException);

                throw CreateAndDependencyValidationException(alreadyExistsLanguageException);
            }
            catch(DbUpdateConcurrencyException DbUpdateConcurrencyException)
            {
                var lockedLanguageException = new LockedLanguageException(DbUpdateConcurrencyException);

                throw CreateAndDependencyValidationException(lockedLanguageException);
            }
            catch(DbUpdateException databaseUpdateException)
            {
                var failedLanguageStorageException = new FailedLanguageStorageException(databaseUpdateException);

                throw CreateAndLogDependencyException(failedLanguageStorageException);
            }
            // catch (Exception exception)
            // {
            //     var failedLanguageServiceException = new FailedLanguageServiceException(exception);

            //     throw CreateAndLogServiceException(failedLanguageServiceException);
            // }
        }

        private IQueryable<Language> TryCatch(ReturningLanguagesFunction returningLanguageFunction)
        {
            try
            {
                return returningLanguageFunction();
            }
            catch (SqlException sqlException)
            {
                var failedLanguageStorageException = new FailedLanguageStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedLanguageStorageException);
            }
            catch (Exception serviceException)
            {
                var failedLanguageServiceException = new FailedLanguageServiceException(serviceException);

                throw CreateAndLogServiceException(failedLanguageServiceException);
            }
        }

        private LanguageDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var languageDependencyException = new LanguageDependencyException(exception);
            this.loggingBroker.LogCritical(languageDependencyException);

            return languageDependencyException;
        }

        private LanguageValidationException CreateAndLogValidationException(Xeption exception)
        {
            var languageValidationException = new LanguageValidationException(exception);

            this.loggingBroker.LogError(languageValidationException);

            throw languageValidationException;
        }

        private LanguageDependencyValidationException CreateAndDependencyValidationException(Xeption exception)
        {

            var languageDependencyValidationException = new LanguageDependencyValidationException(exception);
            this.loggingBroker.LogError(languageDependencyValidationException);

            return languageDependencyValidationException;
        }

        private LanguageServiceException CreateAndLogServiceException(Xeption exception)
        {
            var languageServiceException = new LanguageServiceException(exception);
            this.loggingBroker.LogError(languageServiceException);

            return languageServiceException;
        }
        private LanguageDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var languageDependencyException = new LanguageDependencyException(exception);
            this.loggingBroker.LogError(languageDependencyException);

            return languageDependencyException;
        }
    }
}