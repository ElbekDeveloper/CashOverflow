// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using CashOverflow.Models.Languages;

namespace CashOverflow.Services.Foundations.Languages
{
    public interface ILanguageService
    {
        /// <exception cref="Models.Languages.Exceptions.LanguageServiceException"></exception>
        /// <exception cref="Models.Languages.Exceptions.LanguageDependencyException"></exception>
        /// <exception cref="Models.Languages.Exceptions.LanguageValidationException"></exception>
        /// <exception cref="Models.Languages.Exceptions.LanguageDependencyValidationException"></exception>
        ValueTask<Language> AddLanguageAsync(Language language);

        IQueryable<Language> RetrieveAllLanguages();
        ValueTask<Language> RetrieveLanguageByIdAsync(Guid languageId);
        ValueTask<Language> ModifyLanguageAsync(Language language);
        ValueTask<Language> RemoveLanguageByIdAsync(Guid languageId);
    }
}
