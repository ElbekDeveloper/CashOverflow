// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

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
        public async Task ShouldThrowValidationExceptionOnAddIfInputIsNull()
        {
            // given
            Language nullLanguage = null;
            var nullLanguageException = new NullLanguageException();
            var expectedLanguageValidationException =
                new LanguageValidationException(nullLanguageException);

            // when
            ValueTask<Language> addLanguageTask =
                this.languageService.AddLanguageAsync(nullLanguage);

            LanguageValidationException actualLanguageValidationException =
                await Assert.ThrowsAsync<LanguageValidationException>(
                    addLanguageTask.AsTask);

            // then
            actualLanguageValidationException.Should()
                .BeEquivalentTo(expectedLanguageValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLanguageValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertLanguageAsync(It.IsAny<Language>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddLanguageIsInvalidAndLogItAsync(
            string invalidText)
        {
            //given
            var invalidLanguage = new Language()
            {
                Name = invalidText
            };

            var invalidLanguageException = new InvalidLanguageException();

            invalidLanguageException.AddData(
                key: nameof(Language.Id),
                values: "Id is required");

            invalidLanguageException.AddData(
                key: nameof(Language.Name),
                values: "Text is required");

            invalidLanguageException.AddData(
                key: nameof(Language.CreatedDate),
                values: "Date is required");

            invalidLanguageException.AddData(
                key: nameof(Language.UpdatedDate),
                values: "Date is required");

            var expectedLanguageValidationException =
                new LanguageValidationException(invalidLanguageException);

            //when
            ValueTask<Language> addLanguageTask =
                this.languageService.AddLanguageAsync(invalidLanguage);

            LanguageValidationException actualLanguageValidationException =
                await Assert.ThrowsAsync<LanguageValidationException>(addLanguageTask.AsTask);

            //then
            actualLanguageValidationException.Should()
                .BeEquivalentTo(expectedLanguageValidationException);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedLanguageValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertLanguageAsync(It.IsAny<Language>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreatedDateIsNotSameAsUpdatedDateAndLogItAsync()
        {
            // given
            int randomMinutes = GetRandomNumber();
            DateTimeOffset randomDate = GetRandomDatetimeOffset();
            Language randomLanguage = CreateRandomLanguage(randomDate);
            Language invalidLanguage = randomLanguage;
            invalidLanguage.UpdatedDate = randomDate.AddMinutes(randomMinutes);
            var invalidLanguageException = new InvalidLanguageException();

            invalidLanguageException.AddData(
                key: nameof(Language.CreatedDate),
                values: $"Date is not same as {nameof(Language.UpdatedDate)}");

            var expectedLanguageValidationException = new LanguageValidationException(invalidLanguageException);

            // when
            ValueTask<Language> addLanguageTask = this.languageService.AddLanguageAsync(invalidLanguage);

            LanguageValidationException actualLanguageValidationException =
                await Assert.ThrowsAsync<LanguageValidationException>(addLanguageTask.AsTask);

            //then
            actualLanguageValidationException.Should().BeEquivalentTo(expectedLanguageValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedLanguageValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker => broker.InsertLanguageAsync(It.IsAny<Language>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
