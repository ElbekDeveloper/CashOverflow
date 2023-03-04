// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Threading.Tasks;
using CashOverflow.Brokers.Loggings;
using CashOverflow.Brokers.Storages;
using CashOverflow.Models.Languages;
using CashOverflow.Services.Foundations.Languages.Exceptions;

namespace CashOverflow.Services.Foundations.Languages
{
    public class LanguageService : ILanguageService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;

        public LanguageService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker
            )
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
        }

        public async ValueTask<Language> AddLanguageAsync(Language language)
        {
            try
            {
                if(language is null)
                {
                    throw new NullLanguageException();
                }

                return await this.storageBroker.InsertLanguageAsync(language);

            }
            catch (NullLanguageException nullLanguageException)
            {
                var locationValidationException = new LanguageValidationException(nullLanguageException);
                this.loggingBroker.LogError(locationValidationException);

                throw locationValidationException;
            }
        }
    }
}
