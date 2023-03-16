// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using CashOverflow.Models.Languages;
using CashOverflow.Models.Languages.Exceptions;
using Microsoft.Data.SqlClient;

namespace CashOverflow.Services.Foundations.Languages {
    public partial class LanguageService {
        private static void ValidateLanguageId(Guid languageId) =>
           Validate((Rule: IsInvalid(languageId), Parameter: (nameof(Language.Id))));

        private static dynamic IsInvalid(Guid id) => new {
            Condition = id == default,
            Message = "Id is required"
        };

        private static void ValidateStorageLanguageExist(Language maybeLanguage, Guid languageId) {
            if (maybeLanguage is null) {
                throw new NotFoundLanguageException(languageId);
            }
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations) {
            var invalidLangageException = new InvalidLanguageException();

            foreach ((dynamic rule, string parameter) in validations) {
                if (rule.Condition) {
                    invalidLangageException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }
            invalidLangageException.ThrowIfContainsErrors();
        }
    }
}