using System;
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
        public async Task ShouldThrowValidationExceptionOnRemoveIfInputIdIsNullAndLogItAsync()
        { 
            // given
            Guid nullLanguageId = Guid.NewGuid();
            var nullLanguageException = new NullLanguageException();
            nullLanguageException.AddData(
                key: nameof(Language.Id),
                values: "Id is required");
                
            var expectedLanguageValidationException = new LanguageValidationException(nullLanguageException);
            // when
            ValueTask<Language> removeLanguageByIdTask = 
                this.languageService.RemoveLanguageByIdAsync(nullLanguageId);

            LanguageValidationException actualLanguageValidationException =
                await Assert.ThrowsAsync<LanguageValidationException>(removeLanguageByIdTask.AsTask);

            //then
            actualLanguageValidationException.Should()
                .BeEquivalentTo(expectedLanguageValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                   expectedLanguageValidationException))), Times.Once);
            
            this.storageBrokerMock.Verify(broker =>
                broker.DeleteLanguageAsync(It.IsAny<Language>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
