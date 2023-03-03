// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

namespace CashOverflow.Tests.Unit.Services.Foundations.Languages
{
    public partial class LanguageServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveAllLanguages()
        {
            // given
            IQueryable<Language> randomLanguages = CreatedRandomLanguages();
            IQueryable<Language> storageLanguages = randomLanguages;
            IQueryable<Language> expectedLanguges = storageLanguges;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllLanguages())
                    .Returns(storageLanguages);

            // when
            IQueryable<Language> actualLanguages =
                this.languageServices.RetrieveAllLanguages();

            // then
        }
    }
}
