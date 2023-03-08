using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashOverflow.Models.Languages;
using CashOverflow.Models.Languages.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace CashOverflow.Tests.Unit.Services.Foundations.Languages
{
    public partial class LanguageServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvaledAndLogItAsync()
        {
            //given
            var nullLanguageId = Guid.Empty;
            var nullLanguageIdException = new NulllLanguageIdExcaption();

            var expectedLanguageValidationExcaption = 
                new LanguageValidationExcaption(nullLanguageIdException);


            //when
            ValueTask<Language> retrieveLanguageById =
                this.languageService.RetrieveLanguageByIdAsync(nullLanguageId);

            LanguageValidationExcaption actualLanguageValidationExcaption =
                await Assert.ThrowsAsync<LanguageValidationExcaption>(
                    retrieveLanguageById.AsTask);
            //then
            actualLanguageValidationExcaption.Should().BeEquivalentTo(expectedLanguageValidationExcaption);

            this.loggingBrokerMock.Verify(broker =>
              broker.LogError(It.Is(SameExceptionAs(
                  expectedLanguageValidationExcaption))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLanguageByIdAsync(It.IsAny<Guid>()), Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
