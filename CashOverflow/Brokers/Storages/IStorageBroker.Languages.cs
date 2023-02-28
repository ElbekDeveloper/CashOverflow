// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Threading.Tasks;
using CashOverflow.Models.Languages;

namespace CashOverflow.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Language> SelectLanguageByIdAsync(Guid Id);
    }
}
