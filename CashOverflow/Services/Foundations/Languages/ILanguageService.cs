// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

namespace CashOverflow.Services.Foundations.Languages
{
    public interface ILanguageService
    {
        IQueryable<Language> RetrieveAllLanguages();
    }
}
