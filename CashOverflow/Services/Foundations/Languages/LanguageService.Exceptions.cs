// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System.Threading.Tasks;
using CashOverflow.Models.Languages;
using CashOverflow.Models.Languages.Exceptions;
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
        }

        private LanguageValidationException CreateAndLogValidationException(Xeption exception)
        {
            var locationValidationException = new LanguageValidationException(innerException: exception);
            this.loggingBroker.LogError(locationValidationException);

            return locationValidationException;
        }
    }
}
