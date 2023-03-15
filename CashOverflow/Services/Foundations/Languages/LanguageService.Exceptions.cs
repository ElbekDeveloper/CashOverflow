// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Threading.Tasks;
using CashOverflow.Models.Languages;
using CashOverflow.Models.Languages.Exceptions;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Xeptions;

namespace CashOverflow.Services.Foundations.Languages
{
    public partial class LanguageService
    {
        private delegate ValueTask<Language> ReturningLanguageFunction();

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
            catch (SqlException sqlException)
            {
                var failedLanguageStorageException = new FailedLanguageStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedLanguageStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsLanguageException = new AlreadyExistsLanguageException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsLanguageException);
            }
            catch (Exception exception)
            {
                var failedLanguageServiceException = new FailedLanguageServiceException(exception);

                throw CreateAndLogServiceException(failedLanguageServiceException);
            }
        }

        private LanguageDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var languageDependencyValidationException = new LanguageDependencyValidationException(exception);

            this.loggingBroker.LogError(languageDependencyValidationException);

            return languageDependencyValidationException;
        }

        private LanguageValidationException CreateAndLogValidationException(Xeption exception)
        {
            var languageValidationException = new LanguageValidationException(exception);
            this.loggingBroker.LogError(languageValidationException);

            return languageValidationException;
        }

        private LanguageDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var LanguageDependencyException = new LanguageDependencyException(exception);
            this.loggingBroker.LogCritical(LanguageDependencyException);

            return LanguageDependencyException;
        }

        private LanguageServiceException CreateAndLogServiceException(Xeption exception)
        {
            var languageServiceException = new LanguageServiceException(exception);

            this.loggingBroker.LogError(languageServiceException);

            return languageServiceException;
        }
    }
}
