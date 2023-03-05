// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Linq;
using CashOverflow.Models.Languages;
using CashOverflow.Models.Languages.Exceptions;
using Microsoft.Data.SqlClient;
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
            var LanguageDependencyException =
                new LanguageDependencyException(exception);

            this.loggingBroker.LogCritical(LanguageDependencyException);

            return LanguageDependencyException;
        }

        private LanguageServiceException CreateAndLogServiceException(Xeption exception)
        {
            var LanguageServiceException = new LanguageServiceException(exception);
            this.loggingBroker.LogError(LanguageServiceException);

            return LanguageServiceException;
        }
    }
}
