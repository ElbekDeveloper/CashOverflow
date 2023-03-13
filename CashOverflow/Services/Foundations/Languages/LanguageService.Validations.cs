using System;
using CashOverflow.Models.Languages;
using CashOverflow.Models.Languages.Exceptions;

namespace CashOverflow.Services.Foundations.Languages
{
    public partial class LanguageService
    {
        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == default,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Value is required"
        };

        private void ValidateLanguageId(Guid languageId) =>
           Validate((Rule: IsInvalid(languageId), Parameter: nameof(Language.Id)));

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidLanguageException = new InvalidLanguageException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidLanguageException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidLanguageException.ThrowIfContainsErrors();
        }
        private void ValidateStorageLanguage(Language maybeTicket, Guid languageId)
        {
            if (maybeTicket is null)
            {
                throw new NotFoundLanguageException(languageId);
            }
        }
    }
}
