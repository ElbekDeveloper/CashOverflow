// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System.Threading.Tasks;
using CashOverflow.Models.Languages;
using CashOverflow.Models.Languages.Exceptions;
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
                var FailedLanguageStorageException = new FailedLanguageStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(FailedLanguageStorageException);
            }
        }

        private LanguageValidationException CreateAndLogValidationException(Xeption exception)
        {
            var languageValidationException = new LanguageValidationException(exception);
            this.loggingBroker.LogError(languageValidationException);

            return languageValidationException;
        }

        private LanguageDependencyException CreateAndLogCriticalDependencyException(Xeption exeption)
        {
            var LanguageDependencyException = new LanguageDependencyException(exeption);
            this.loggingBroker.LogCritical(LanguageDependencyException);

            return LanguageDependencyException;
        }
    }
}
