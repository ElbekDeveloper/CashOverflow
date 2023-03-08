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
        IQueryable<Language> RetrieveAllLanguages();
        ValueTask<Language> RetrieveByIdLanguages(Guid languageId);
        
    }
}
