using System;
using System.Linq;
using CashOverflow.Brokers.Storages;
using CashOverflow.Models.Languages;

namespace CashOverflow.Services.Foundations.Languages
{
    public class LanguageService : ILanguageService
    {
        private IStorageBroker storageBroker;

        public LanguageService(IStorageBroker storageBroker) =>
              this.storageBroker = storageBroker;

        public IQueryable<Language> RetrieveAllLanguages() =>
            throw new NotImplementedException();
            //TryCatch(() => this.storageBroker.SelectAllLanguages());

    }
}
