// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

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
        public async Task ShouldThrowValidationExceptionOnModifyIfLanguageIsNullAndLogItAsync()
        {
            //given
            Language nullLanguage = null;
            var nullLanguageException = new NullLanguageException();

            var excpectedLanguageValidationException =
                new LanguageValidationException(nullLanguageException);

            //when
            ValueTask<Language> modifyLanguageTask =
                this.languageService.ModifyLanguageAsync(nullLanguage);

            LanguageValidationException actualLanguageValidationException =
                await Assert.ThrowsAsync<LanguageValidationException>(modifyLanguageTask.AsTask);

            //then
            actualLanguageValidationException.Should().BeEquivalentTo(excpectedLanguageValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(excpectedLanguageValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateLanguageAsync(It.IsAny<Language>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
