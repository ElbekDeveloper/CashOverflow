// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System.Net.Sockets;
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

        [Theory]
        [InlineData(null)]
        [InlineData(" ")]
        [InlineData("  ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfLanguageIsInvalidAndLogItAsync(
            string invalidString)
        {
            //given
            var invalidLanguage = new Language
            {
                Name = invalidString
            };

            var invalidLanguageException = new InvalidLanguageException();

            invalidLanguageException.AddData(
                key: nameof(Language.Id),
                values: "Id is required");

            invalidLanguageException.AddData(
                key: nameof(Language.Name),
                values: "Language name is required");

            invalidLanguageException.AddData(
                key: nameof(Language.Type),
                values: "Type is required");

            invalidLanguageException.AddData(
                key: nameof(Language.CreatedDate),
                values: "Value is required");

            invalidLanguageException.AddData(
                key:nameof(Language.UpdatedDate),
                 "Value is required",
                "Date is not recent",
                $"Date is the same as {nameof(Language.CreatedDate)}");

            var excpectedLanguageValidationException =
                new LanguageValidationException(invalidLanguageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Returns(GetRandomDatetimeOffset); 

            //when
            ValueTask<Language>modifyLanguageTask = this.languageService.ModifyLanguageAsync(invalidLanguage);

            LanguageValidationException actualLanguageValidationException =
                await Assert.ThrowsAsync<LanguageValidationException>(modifyLanguageTask.AsTask);

            //then
            actualLanguageValidationException.Should().BeEquivalentTo(excpectedLanguageValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(excpectedLanguageValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateLanguageAsync(It.IsAny<Language>()), Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
