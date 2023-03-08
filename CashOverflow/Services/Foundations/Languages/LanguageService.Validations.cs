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
        private static void ValidateLanguageOnAdd(Language language)
        {
            ValidateLanguageNotNull(language);

            Validate(
                (Rule: IsInvalid(language.Id), Parameter: nameof(Language.Id)),
                (Rule: IsInvalid(language.Name), Parameter: nameof(Language.Name)),
                (Rule: IsInvalid(language.CreatedDate), Parameter: nameof(Language.CreatedDate)),
                (Rule: IsInvalid(language.UpdatedDate), Parameter: nameof(Language.UpdatedDate)),

                (Rule: IsInvalid(
                    firstDate: language.CreatedDate,
                    secondDate: language.UpdatedDate,
                    secondDateName: nameof(Language.UpdatedDate)),

                    Parameter: nameof(Language.CreatedDate)));
        }

        private static void ValidateLanguageNotNull(Language language)
        {
            if (language is null)
            {
                throw new NullLanguageException();
            }
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate != secondDate,
                Message = $"Date is not same as {secondDateName}"
            };

        private static dynamic IsInvalid(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };

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
    }
}
