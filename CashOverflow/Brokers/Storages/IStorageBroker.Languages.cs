// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using CashOverflow.Models.Languages;

namespace CashOverflow.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        IQueryable<Language> SelectAllLanguages();
        ValueTask<Language> DeleteLanguageAsync(Language language);
    }
}