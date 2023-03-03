// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

namespace CashOverflow.Services.Foundations.Languages
{
    public class LanguageService : ILanguageService
    {
        private readonly IStrogeBroker strogeBroker();
        private readonly ILoggingBroker loggingBroker();

        public LanguageService(
          IStorageBroker storageBroker,
          ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
        }

        IQueryable<Language> RetrieveAllLanguages() =>
            throw new NotImplementedException();
    }
}
