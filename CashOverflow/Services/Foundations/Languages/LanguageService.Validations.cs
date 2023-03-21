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

                (Rule: IsNotSame(
                    firstDate: language.CreatedDate,
                    secondDate: language.UpdatedDate,
                    secondDateName: nameof(Language.UpdatedDate)),

                    Parameter: nameof(Language.CreatedDate)));
        }

        private void ValidateLanguageOnModify(Language language)
        {
            ValidateLanguageNotNull(language);

            Validate(
                (Rule: IsInvalid(language.Id), Parameter: nameof(Language.Id)),
                (Rule: IsInvalid(language.Name), Parameter: nameof(Language.Name)),
                (Rule: IsInvalid(language.Type), Parameter: nameof(Language.Type)),
                (Rule: IsInvalid(language.CreatedDate), Parameter: nameof(Language.CreatedDate)),
                (Rule: IsInvalid(language.UpdatedDate), Parameter: nameof(Language.UpdatedDate)),
                (Rule: IsNotRecent(language.UpdatedDate), Parameter: nameof(Language.UpdatedDate)),

                (Rule: IsSame(
                    firstDate: language.UpdatedDate,
                    secondDate: language.CreatedDate,
                    secondDateName: nameof(language.CreatedDate)),
                    Parameter: nameof(language.UpdatedDate)));
        }

        private static void ValidateAgainstStorageLanguageOnModify(Language inputLanguage, Language storageLanguage)
        {
            ValidateStorageLanguage(storageLanguage, inputLanguage.Id);
            Validate(
            (Rule: IsNotSame(
                    firstDate: inputLanguage.CreatedDate,
                    secondDate: storageLanguage.CreatedDate,
                    secondDateName: nameof(Language.CreatedDate)),
                    Parameter: nameof(Language.CreatedDate)),

                     (Rule: IsSame(
                        firstDate: inputLanguage.UpdatedDate,
                        secondDate: storageLanguage.UpdatedDate,
                        secondDateName: nameof(Language.UpdatedDate)),
                        Parameter: nameof(Language.UpdatedDate)));
        }

        private static void ValidateStorageLanguage(Language maybeLanguage, Guid languageId)
        {
            if (maybeLanguage is null)
            {
                throw new NotFoundLanguageException(languageId);
            }
        }

        private void ValidateLanguageId(Guid languageId) =>
             Validate((Rule: IsInvalid(languageId), Parameter: nameof(Language.Id)));

        private void ValidateLanguageNotNull(Language language)
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

        private static dynamic IsNotSame(
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

        private static dynamic IsInvalid<T>(T value) => new
        {
            Condition = IsEnumInvalid(value),
            Message = "Value is not recognized"
        };

        private dynamic IsNotRecent(DateTimeOffset date) => new
        {
            Condition = IsDateNotRecent(date),
            Message = "Date is not recent"
        };

        private static bool IsEnumInvalid<T>(T value)
        {
            bool isDefined = Enum.IsDefined(typeof(T), value);

            return isDefined is false;
        }

        private static dynamic IsSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate == secondDate,
                Message = $"Date is the same as {secondDateName}"
            };

        private bool IsDateNotRecent(DateTimeOffset date)
        {
            DateTimeOffset currentDate = this.dateTimeBroker.GetCurrentDateTimeOffset();
            TimeSpan timeDifference = currentDate.Subtract(date);

            return timeDifference.TotalSeconds is > 60 or < 0;
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidLangageException = new InvalidLanguageException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidLangageException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }
            invalidLangageException.ThrowIfContainsErrors();
        }
    }
}
