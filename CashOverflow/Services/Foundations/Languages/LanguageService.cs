// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

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

        public ValueTask<Language> AddLanguageAsync(Language language) =>
        TryCatch(async () =>
        {
            ValidateLanguageOnAdd(language);

            return await this.storageBroker.InsertLanguageAsync(language);
        });
    }
}
