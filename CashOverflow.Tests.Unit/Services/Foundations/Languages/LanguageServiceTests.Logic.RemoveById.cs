using System;
using System.Threading.Tasks;
using CashOverflow.Models.Languages;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Xunit;

namespace CashOverflow.Tests.Unit.Services.Foundations.Languages {
    public partial class LanguageServiceTests {
        [Fact]
        public async Task ShouldRemoveLanguageByIdAsync() {
            //given
            Guid randomId = Guid.NewGuid();
            Guid inputLanguageId = randomId;
            Language randomLanguage = CreateRandomLanguage();
            Language storageLanguage = randomLanguage;
            Language expectedInputLanguage = storageLanguage;
            Language deletedLanguage = expectedInputLanguage;
            Language expectedLanguage = deletedLanguage.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectLanguageByIdAsync(inputLanguageId))
                     .ReturnsAsync(storageLanguage);

            this.storageBrokerMock.Setup(broker =>
                 broker.DeleteLanguageAsync(expectedInputLanguage))
                    .ReturnsAsync(deletedLanguage);

            //when
            Language actualLanguage = await
                this.languageService.RemoveLanguageByIdAsync(inputLanguageId);

            //then
            actualLanguage.Should().BeEquivalentTo(expectedLanguage);

            this.storageBrokerMock.Verify(broker =>
            broker.SelectLanguageByIdAsync(inputLanguageId), Times.Once());

            this.storageBrokerMock.Verify(broker =>
            broker.DeleteLanguageAsync(expectedInputLanguage), Times.Once());

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
