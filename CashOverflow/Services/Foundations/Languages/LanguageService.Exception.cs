// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using CashOverflow.Models.Languages;
using CashOverflow.Models.Languages.Exceptions;
using Microsoft.Data.SqlClient;
using Xeptions;

namespace CashOverflow.Services.Foundations.Languages
{
    public partial class LanguageService
    {
        private delegate IQueryable<Language> ReturningLanguagesFunction();
        private delegate ValueTask<Language> ReturningLanguageFunction();
        private IQueryable<Language> TryCatch(ReturningLanguagesFunction returningLanguagesFunction)
        {
            try
            {
                return returningLanguagesFunction();
            }
            catch (SqlException sqlException)
            {
                var failedLanguageStorageException =
                     new FailedLanguageStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedLanguageStorageException);
            }
            catch (Exception exception)
            {
                var failedLanguageServiceException =
                    new FailedLanguageServiceException(exception);

                throw CreateAndLogServiceException(failedLanguageServiceException);
            }
        }

        private async ValueTask<Language> TryCatch(ReturningLanguageFunction returningLanguageFunction)
        {
            try
            {
                return await returningLanguageFunction();
            }
            catch (InvalidLanguageException nullLanguageIdExcaption)
            {
                throw CreateAndLogValidationException(nullLanguageIdExcaption);
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
            catch (Exception serviceException)
            {
                var failedServiceProfileException = new FailedLanguageServiceException(serviceException);

                throw CreateAndLogServiceException(failedServiceProfileException);
            }
        }

        private LanguageDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var languageDependencyException =
                new LanguageDependencyException(exception);

            this.loggingBroker.LogCritical(languageDependencyException);

            return languageDependencyException;
        }

        private LanguageServiceException CreateAndLogServiceException(Xeption exception)
        {
            var languageServiceException = new LanguageServiceException(exception);
            this.loggingBroker.LogError(languageServiceException);

            return languageServiceException;
        }

        private LanguageValidationException CreateAndLogValidationException(Xeption excaption)
        {
            var languageValidationExcaption = new LanguageValidationException(excaption);
            this.loggingBroker.LogError(languageValidationExcaption);

            return languageValidationExcaption;
        }
    }
}
