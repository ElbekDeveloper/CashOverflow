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

        public ValueTask<Language> AddLanguageAsync(Language language) =>
        TryCatch(async () =>
        {
            ValidateLanguageOnAdd(language);

            return await this.storageBroker.InsertLanguageAsync(language);
        });
    }
}
