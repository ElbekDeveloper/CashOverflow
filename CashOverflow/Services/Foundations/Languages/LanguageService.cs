// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

<<<<<<< HEAD
using System.Linq;
=======
>>>>>>> cd307bc66517cf430e3e32d96cdfcc10dbe2ca48
using CashOverflow.Brokers.Loggings;
using CashOverflow.Brokers.Storages;
using CashOverflow.Models.Languages;
using System.Linq;

namespace CashOverflow.Services.Foundations.Languages
{
    public class LanguageService : ILanguageService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;

        public LanguageService(
          IStorageBroker storageBroker,
          ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
        }

        public IQueryable<Language> RetrieveAllLanguages()
        {
            IQueryable<Language> storageLanguages = this.storageBroker.SelectAllLanguages();

            return storageLanguages;
        }

    }
}
