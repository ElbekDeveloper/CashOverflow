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
        public async Task ShouldThrowValidationExceptionOnRemoveIfInputIdIsNullAndLogItAsync()
        {
            // given
            Guid nullLanguageId = Guid.Empty;
            var invalidLanguageException = new InvalidLanguageException();

            invalidLanguageException.AddData(
                key: nameof(Language.Id),
                values: "Id is required");

            var expectedLanguageValidationException =
                new LanguageValidationException(invalidLanguageException);

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

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRemoveIfLanguageIsNotFoundAndLogItAsync()
        {
            //given
            Guid randomLanguageId = Guid.NewGuid();
            Guid inputLanguageId = randomLanguageId;
            Language noLanguage = null;
            var notFoundLanguageException = new NotFoundLanguageException(inputLanguageId);

            var expectedLanguageValidationException =
                new LanguageValidationException(notFoundLanguageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectLanguageByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(noLanguage);

            //when
            ValueTask<Language> removeLanguageByIdTask =
                this.languageService.RemoveLanguageByIdAsync(inputLanguageId);

            LanguageValidationException actualLanguageValidationException =
                await Assert.ThrowsAsync<LanguageValidationException>(removeLanguageByIdTask.AsTask);

            //then
            actualLanguageValidationException.Should()
                .BeEquivalentTo(expectedLanguageValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLanguageByIdAsync(It.IsAny<Guid>()), Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLanguageValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteLanguageAsync(It.IsAny<Language>()), Times.Never());

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
