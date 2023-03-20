﻿// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System.Linq;
using CashOverflow.Brokers.Loggings;
using CashOverflow.Brokers.Storages;
using CashOverflow.Models.Languages;

namespace CashOverflow.Services.Foundations.Languages
{
    public partial class LanguageService : ILanguageService
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

        public IQueryable<Language> RetrieveAllLanguages() =>
<<<<<<< HEAD
            TryCatch(() => this.storageBroker.SelectAllLanguages());
=======
        TryCatch(() => this.storageBroker.SelectAllLanguages());

        public ValueTask<Language> RetrieveLanguageByIdAsync(Guid languageId) =>
        TryCatch(async () =>
        {
            ValidateLanguageId(languageId);

            Language maybeLanguage =
                await this.storageBroker.SelectLanguageByIdAsync(languageId);

            ValidateStorageLanguage(maybeLanguage, languageId);

                return maybeLanguage;
            });

        public ValueTask<Language> RemoveLanguageByIdAsync(Guid languageId) =>
        TryCatch(async () =>
        {
            ValidateLanguageId(languageId);

            Language maybeLanguage = await this.storageBroker.
                SelectLanguageByIdAsync(languageId);

            ValidateStorageLanguageExist(maybeLanguage, languageId);

            return await this.storageBroker.DeleteLanguageAsync(maybeLanguage);
        });
>>>>>>> 616ecb1c0aef2aa1d1b94db757dc1fde39cf606f
    }
}
