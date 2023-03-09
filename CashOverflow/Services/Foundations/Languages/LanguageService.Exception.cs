// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using CashOverflow.Models.Languages;
using CashOverflow.Models.Languages.Exceptions;
using Microsoft.Data.SqlClient;
using System;
using System.Linq;
using Xeptions;

namespace CashOverflow.Services.Foundations.Languages
{
    public partial class LanguageService
    {
        private delegate IQueryable<Language> ReturningLanguagesFunction();

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
    }
}
