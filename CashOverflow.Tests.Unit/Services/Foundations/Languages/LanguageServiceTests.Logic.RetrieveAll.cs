// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using CashOverflow.Models.Languages;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Xunit;

namespace CashOverflow.Tests.Unit.Services.Foundations.Languages
{
    public partial class LanguageServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllLanguages()
        {
            // given
            IQueryable<Language> randomLanguages = CreateRandomLanguages();
            IQueryable<Language> storageLanguages = randomLanguages;
            IQueryable<Language> expectedLanguges = storageLanguages.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllLanguages())
                    .Returns(storageLanguages);

            // when
            IQueryable<Language> actualLanguages =
                this.languageService.RetrieveAllLanguages();

            // then
            actualLanguages.Should().BeEquivalentTo(expectedLanguges);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllLanguages(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
