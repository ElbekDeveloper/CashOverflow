// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System.Linq;
using CashOverflow.Models.Languages;
using FluentAssertions;
using Moq;
using Xunit;

namespace CashOverflow.Tests.Unit.Services.Foundations.Languages
{
    public partial class LanguageServiceTests
    {
        [Fact]
        public void ShouldSelectAllLanguages()
        {
            // given
            IQueryable<Language> randomLanguages = CreateRandomLanguages();
            IQueryable<Language> storageLanguages = randomLanguages;
            IQueryable<Language> expectedLanguages = storageLanguages;

            this.storageBrokerMock.Setup(broker =>
            broker.SelectAllLanguages()).Returns(expectedLanguages);

            // when
            IQueryable<Language> actualLanguages = this.languageService.RetrieveAllLanguages();

            // then
            actualLanguages.Should()
                .BeEquivalentTo(expectedLanguages);

            this.storageBrokerMock.Verify(broker =>
             broker.SelectAllLanguages(), Times.Once());

            this.storageBrokerMock.VerifyNoOtherCalls();
        }


    }
}
