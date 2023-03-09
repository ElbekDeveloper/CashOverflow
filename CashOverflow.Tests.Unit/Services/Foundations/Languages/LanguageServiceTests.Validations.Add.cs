using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CashOverflow.Models.Languages;
using CashOverflow.Services.Foundations.Languages;
using CashOverflow.Services.Foundations.Languages.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace CashOverflow.Tests.Unit.Services.Foundations.Languages
{
    public partial class LanguageServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfInputIsNulAndLogItAsync()
        { 
            // given
            Guid nullLanguageId = Guid.Empty;
            var nullLanguageException = new NullLanguageException();
            var expectedLanguageValidationException = new LanguageValidationException(nullLanguageException);
            // when
            ValueTask<Language> removeLanguageTask = this.languageService.RemoveLanguageByIdAsync(nullLanguageId);

            LanguageValidationException actualLanguageValidationException =
                await Assert.ThrowsAsync<LanguageValidationException>(removeLanguageTask.AsTask);
            // then

            actualLanguageValidationException.Should()
                .BeEquivalentTo(expectedLanguageValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedLanguageValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteLanguageAsync(It.IsAny<Language>()), Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

    }
}
