// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using CashOverflow.Models.Languages;

namespace CashOverflow
{
    public interface ILanguageService
    {
        ValueTask<Language> AddLanguageAsync(Language language);
        IQueryable<Language> RetrieveAllLanguages();
    }
}
