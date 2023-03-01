// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Threading.Tasks;
using CashOverflow.Models.Languages;
using Microsoft.EntityFrameworkCore;

namespace CashOverflow.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Language> Languages { get; set; }

        public async ValueTask<Language> InsertLanguageAsync(Language language) =>
            await this.InsertAsync(language);

        public ValueTask<Language> SelectLanguageByIdAsync(Guid languageId) =>
           SelectAsync<Language>(languageId);

        public async ValueTask<Language> UpdateLanguageAsync(Language language) =>
            await UpdateAsync(language);

        public async ValueTask<Language> DeleteLanguageAsync(Language language) =>
            await DeleteAsync<Language>(language);
    }
}
