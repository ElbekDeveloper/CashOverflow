// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using CashOverflow.Models.Languages;

namespace CashOverflow.Services.Foundations.Languages
{
    public partial class LanguageService
    {
        private void ValidateAgainstStorageLanguageOnModify(Language inputLanguage, Language storageLanguage)
        {
            throw new NotImplementedException();
        }

        private object ValidateLanguageOnModify(Language language)
        {
            throw new NotImplementedException();
        }

        private static void ValidateLanguageNotNull(Language language)
        {
            if (language is null)
            {
                throw new NullLanguageException();
            }
        }
    }
}
