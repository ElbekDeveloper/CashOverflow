﻿// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using CashOverflow.Models.Languages;

namespace CashOverflow
{
    public interface ILanguageService
    {
        ValueTask<Language> AddLanguageAsync(Language language);
        IQueryable<Language> RetrieveAllLanguages();
        ValueTask<Language> RetrieveLanguageByIdAsync(Guid languageId);
        ValueTask<Language> RemoveLanguageByIdAsync(Guid languageId);
    }
}
