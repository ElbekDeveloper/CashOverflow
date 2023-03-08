// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Linq;
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
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;
        public LanguageService(
          IStorageBroker storageBroker,
          IDateTimeBroker dateTimeBroker,
          ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public IQueryable<Language> RetrieveAllLanguages() =>
            TryCatch(() => this.storageBroker.SelectAllLanguages());

        public ValueTask<Language> RetrieveLanguageByIdAsync(Guid languageId) =>
            TryCatch(async () =>
            {
                ValidateLanguageNotNull(languageId);

                return await this.storageBroker.SelectLanguageByIdAsync(languageId);
            });
    }
}
