// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System.Linq;
using CashOverflow.Models.Languages;

namespace CashOverflow.Services.Foundations.Languages
{
    public interface ILanguageService
    {
        IQueryable<Language> RetrieveAllLanguages();
    }
}
