// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System.Threading.Tasks;
using CashOverflow.Models.Languages;

namespace CashOverflow.Brokers.Storages
{
    public interface ISrorageBroker
    {
        ValueTask<Language> InsertLanguageAsync(Language language);      
    }
}
