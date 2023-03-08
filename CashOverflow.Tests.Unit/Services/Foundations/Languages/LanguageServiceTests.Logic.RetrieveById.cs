// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public async void ShouldRetrieveByIdLanguage()
        {
            // given
            Guid randomLanguageId = Guid.NewGuid();
            Guid inputLanguageId = randomLanguageId;
            Language randomLanguage = CreateRandomLanguage();
            Language storageLanguage = randomLanguage;
            Language expectedLanguage = storageLanguage.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectLanguageByIdAsync(inputLanguageId)).ReturnsAsync(storageLanguage);
            // when
            Language actualLanguage = await this.languageService.RetrieveByIdLanguages(inputLanguageId);


            // then
            actualLanguage.Should().BeEquivalentTo(expectedLanguage);

            this.storageBrokerMock.Verify(broker => 
                broker.SelectLanguageByIdAsync(inputLanguageId),Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
