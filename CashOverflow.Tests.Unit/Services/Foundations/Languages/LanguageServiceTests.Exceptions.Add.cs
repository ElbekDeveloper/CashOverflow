// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Threading.Tasks;
using CashOverflow.Models.Languages;
using CashOverflow.Models.Languages.Exceptions;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace CashOverflow.Tests.Unit.Services.Foundations.Languages
{
    public partial class LanguageServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfDependencyErrorOccursAndLogItAsync()
        {
            // given
            Language someLanguage = CreateRandomLanguage();
            SqlException sqlException = CreateSqlException();
            var failedLanguageStorageException = new FailedLanguageStorageException(sqlException);
            var expectedLanguageDependencyException = new LanguageDependencyException(failedLanguageStorageException);

            this.dateTimeBrokerMock.Setup(broker => broker.GetCurrentDateTimeOffset()).Throws(sqlException);

            // when
            ValueTask<Language> addLanguageTask = this.languageService.AddLanguageAsync(someLanguage);

            LanguageDependencyException actualLanguageDependencyException =
                await Assert.ThrowsAsync<LanguageDependencyException>(addLanguageTask.AsTask);

            //then
            actualLanguageDependencyException.Should().BeEquivalentTo(expectedLanguageDependencyException);

            this.dateTimeBrokerMock.Verify(broker => broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker => broker.LogCritical(
                It.Is(SameExceptionAs(expectedLanguageDependencyException))), Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfDuplicateKeyErrorOccursAndLogItAsync()
        {
            // given
            string someMessage = GetRandomString();
            Language someLanguage = CreateRandomLanguage();
            DuplicateKeyException duplicateKeyException = new DuplicateKeyException(someMessage);
            var alreadyExistsLanguageException = new AlreadyExistsLanguageException(duplicateKeyException);

            var expectedLanguageDependencyValidationException =
                new LanguageDependencyValidationException(alreadyExistsLanguageException);

            this.dateTimeBrokerMock.Setup(broker => broker.GetCurrentDateTimeOffset())
                .Throws(duplicateKeyException);

            // when
            ValueTask<Language> addLanguageTask = this.languageService.AddLanguageAsync(someLanguage);

            LanguageDependencyValidationException actualLanguageDependencyValidationException =
                await Assert.ThrowsAsync<LanguageDependencyValidationException>(addLanguageTask.AsTask);

            // then
            actualLanguageDependencyValidationException.Should().
                BeEquivalentTo(expectedLanguageDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker => broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker => broker.LogError(It.Is(SameExceptionAs(
                expectedLanguageDependencyValidationException))), Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Language someLanguage = CreateRandomLanguage();
            var serviceException = new Exception();
            var failedLanguageServiceException = new FailedLanguageServiceException(serviceException);

            var expectedLanguageServiceException =
                new LanguageServiceException(failedLanguageServiceException);

            this.dateTimeBrokerMock.Setup(broker => broker.GetCurrentDateTimeOffset())
                .Throws(serviceException);

            // when
            ValueTask<Language> addLanguageTask = this.languageService.AddLanguageAsync(someLanguage);

            LanguageServiceException actualLanguageServiceException =
                await Assert.ThrowsAsync<LanguageServiceException>(addLanguageTask.AsTask);

            // then
            actualLanguageServiceException.Should().BeEquivalentTo(expectedLanguageServiceException);

            this.dateTimeBrokerMock.Verify(broker => broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedLanguageServiceException))), Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
