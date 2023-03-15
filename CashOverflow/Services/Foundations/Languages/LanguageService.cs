// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CashOverflow.Brokers.DateTimes;
using CashOverflow.Brokers.Loggings;
using CashOverflow.Brokers.Storages;
using CashOverflow.Models.Languages;

namespace CashOverflow.Services.Foundations.Languages
{
    public partial class LanguageService : ILanguageService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        public LanguageService(
          IStorageBroker storageBroker,
          ILoggingBroker loggingBroker,
          IDateTimeBroker dateTimeBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public IQueryable<Language> RetrieveAllLanguages() =>
            TryCatch(() => this.storageBroker.SelectAllLanguages());

        public ValueTask<Language> ModifyLanguageAsync(Language language) =>
            TryCatch(async () =>
            {
                ValidateLanguageOnModify(language);

                var maybeLanguage =
                    await this.storageBroker.SelectLanguageByIdAsync(language.Id);

                ValidateAgainstStorageLanguageOnModify(inputLanguage: language, storageLanguage: maybeLanguage);

                return await this.storageBroker.UpdateLanguageAsync(language);

            });
    }
}
