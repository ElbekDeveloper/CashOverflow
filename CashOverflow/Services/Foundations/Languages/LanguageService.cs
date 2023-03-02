// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System.Threading.Tasks;
using CashOverflow.Brokers.Storages;
using CashOverflow.Models.Languages;

namespace CashOverflow.Services.Foundations.Languages
{
    public class LanguageService : ILanguageService
    {
        private IStorageBroker storageBroker;

        public LanguageService(IStorageBroker storageBroker) =>
            this.storageBroker = storageBroker;

        public async ValueTask<Language> AddLanguageAsync(Language language) =>
            throw new System.NotImplementedException();

    }
}
