// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Threading.Tasks;
using CashOverflow.Brokers.Storages;
using CashOverflow.Models.Languages;

namespace CashOverflow.Services.Foundations.Languages
{
    public class LanguageService : ILanguageService
    {
        private readonly IStorageBroker storageBroker;

        public LanguageService(IStorageBroker storageBroker) =>
            this.storageBroker = storageBroker;

        public ValueTask<Language> AddLanguageAsync(Language language) =>
            this.storageBroker.InsertLanguageAsync(language);
    }
}
