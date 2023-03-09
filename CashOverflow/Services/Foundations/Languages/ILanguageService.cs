// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using CashOverflow.Models.Languages;
using System.Linq;

namespace CashOverflow.Services.Foundations.Languages
{
    public interface ILanguageService
    {
        IQueryable<Language> RetrieveAllLanguages();
    }
}
