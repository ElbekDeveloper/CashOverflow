// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CashOverflow.Models.Languages;

namespace CashOverflow.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Language> InsertLanguageAsync(Language language);
        ValueTask<Language> UpdateLanguageAsync(Language language);
        ValueTask<Language> DeleteLanguageAsync(Language language);
    }
}