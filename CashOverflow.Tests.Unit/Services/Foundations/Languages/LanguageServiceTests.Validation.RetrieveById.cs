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
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            //given
            var invalideLanguageId = Guid.Empty;
            var invalidLanguageException = new InvalidLanguageException();

            invalidLanguageException.AddData(
             key: nameof(Language.Id),
             values: "Id is required");

            var expectedLanguageValidationExcaption =
                new LanguageValidationException(invalidLanguageException);

            //when
            ValueTask<Language> retrieveLanguageById =
                this.languageService.RetrieveLanguageByIdAsync(invalideLanguageId);

            LanguageValidationException actualLanguageValidationExcaption =
                await Assert.ThrowsAsync<LanguageValidationException>(
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

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfLanguageNotFoundAndLogItAsync()
        {
            //given
            Guid someLanguageId = Guid.NewGuid();
            Language noLanguage = null;

            var notFoundLanguageValidationException =
                new NotFoundLanguageException(someLanguageId);

            var expectedLanguageValidationExcaption =
                new LanguageValidationException(notFoundLanguageValidationException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectLanguageByIdAsync(It.IsAny<Guid>())).ReturnsAsync(noLanguage);

            //when
            ValueTask<Language> retrieveByIdLanguageTask =
                this.languageService.RetrieveLanguageByIdAsync(someLanguageId);

            LanguageValidationException actualLanguageValidationExcaption =
                await Assert.ThrowsAsync<LanguageValidationException>(
                    retrieveByIdLanguageTask.AsTask);

            //then
            actualLanguageValidationExcaption.Should().BeEquivalentTo(expectedLanguageValidationExcaption);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLanguageByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLanguageValidationExcaption))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
