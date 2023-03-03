// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

namespace CashOverflow.Tests.Unit.Services.Foundations.Languages
{
    public partial class LanguageServiceTests
    {
        private readonly Mock<IStrogeBroker> strogeBrokerMock();
        private readonly Mock<ILoggingBroker> loggingBrokerMock();
        private readonly ILanguageService languageService;

        public LanguageServiceTests()
        {
            this.strogeBrokerMock = new Mock<IStrogeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.languageService = new LanguageService
                (
                strogeBroker: this.strogeBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object
                );
        }
    }
}
