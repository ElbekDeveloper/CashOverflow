// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
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
        public async Task ShouldModifyLanguageAsync()
        {
            //given
            DateTimeOffset randomDate = GetRandomDatetimeOffset();
            Language randomLanguage = CreateRandomModifyLanguage(randomDate);
            Language inputLanguage = randomLanguage;
            Language storageLangauge = inputLanguage.DeepClone();
            storageLangauge.UpdatedDate = randomLanguage.CreatedDate;
            Language updatedLanguage = inputLanguage;
            Language expectedLanguage = updatedLanguage.DeepClone();
            Guid languageId = inputLanguage.Id;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Returns(randomDate);
            
            this.storageBrokerMock.Setup(broker =>
                broker.SelectLanguageByIdAsync(languageId))
                    .ReturnsAsync(storageLangauge);
            
            this.storageBrokerMock.Setup(broker =>
                broker.UpdateLanguageAsync(inputLanguage))
                    .ReturnsAsync(updatedLanguage);
            
            //when
            Language actualLanguage =
                await this.languageService.ModifyLanguageAsync(inputLanguage);
            
            //then
            actualLanguage.Should().BeEquivalentTo(expectedLanguage);
            
            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);
            
            this.storageBrokerMock.Verify(broker =>
                broker.SelectLanguageByIdAsync(languageId), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateLanguageAsync(inputLanguage), Times.Once);
            
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
