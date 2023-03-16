// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using CashOverflow.Models.Languages;
using CashOverflow.Models.Languages.Exceptions;

namespace CashOverflow.Services.Foundations.Languages
{
    public partial class LanguageService
    {
        private void ValidateLanguageOnAdd(Language language)
        {
            ValidateLanguageNotNull(language);

            Validate(
                (Rule: IsInvalid(language.Id), Parameter: nameof(Language.Id)),
                (Rule: IsInvalid(language.Name), Parameter: nameof(Language.Name)),
                (Rule: IsInvalid(language.CreatedDate), Parameter: nameof(Language.CreatedDate)),
                (Rule: IsInvalid(language.UpdatedDate), Parameter: nameof(Language.UpdatedDate)),
                (Rule: IsNotRecent(language.CreatedDate), Parameter: nameof(Language.CreatedDate)),

                (Rule: IsInvalid(
                    firstDate: language.CreatedDate,
                    secondDate: language.UpdatedDate,
                    secondDateName: nameof(Language.UpdatedDate)),

                    Parameter: nameof(Language.CreatedDate)));
        }

        private void ValidateLanguageNotNull(Language language)
        {
            if (language is null)
            {
                throw new NullLanguageException();
            }
        }

        private dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private dynamic IsInvalid(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate != secondDate,
                Message = $"Date is not same as {secondDateName}"
            };

        private dynamic IsInvalid(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };

        private dynamic IsNotRecent(DateTimeOffset date) => new
        {
            Condition = IsDateNotRecent(date),
            Message = "Date is not recent"
        };

        private bool IsDateNotRecent(DateTimeOffset date)
        {
            DateTimeOffset currentDate = this.dateTimeBroker.GetCurrentDateTimeOffset();
            TimeSpan timeDifference = currentDate.Subtract(date);

            return timeDifference.TotalSeconds is > 60 or < 0;
        }

        private void Validate(params (dynamic Rule, string Parameter)[] validations)
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

        private void ValidateLanguageId(Guid languageId) =>
             Validate((Rule: IsInvalid(languageId), Parameter: nameof(Language.Id)));
    }
}
