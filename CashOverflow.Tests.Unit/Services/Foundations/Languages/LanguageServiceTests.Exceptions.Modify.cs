// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Threading.Tasks;
using CashOverflow.Models.Languages;
using CashOverflow.Models.Languages.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace CashOverflow.Tests.Unit.Services.Foundations.Languages
{
    public partial class LanguageServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            //given
            DateTimeOffset someDateTime = GetRandomDatetimeOffset();
            Language randomLanguage = CreateRandomLanguage(someDateTime);
            Language someLanguage = randomLanguage;
            Guid languageId = someLanguage.Id;
            SqlException sqlException = CreateSqlException();

            var failedLanguageStorageException =
                new FailedLanguageStorageException(sqlException);

            var expectedLanguageDependencyException =
                new LanguageDependencyException(failedLanguageStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Throws(sqlException);

            //when
            ValueTask<Language> modifyLanguageTask =
                this.languageService.ModifyLanguageAsync(someLanguage);

            LanguageDependencyException actualLanguageDependencyException =
                await Assert.ThrowsAsync<LanguageDependencyException>(
                    modifyLanguageTask.AsTask);

            //then
            actualLanguageDependencyException.Should().BeEquivalentTo(
                    expectedLanguageDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedLanguageDependencyException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLanguageByIdAsync(languageId), Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateLanguageAsync(someLanguage), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            //given
            int minutesInPast = GetRandomNegativeNumber();
            DateTimeOffset randomDateTime = GetRandomDatetimeOffset();
            Language randomLanguage = CreateRandomLanguage(randomDateTime);
            Language someLanguage = randomLanguage;
            Guid languageId = someLanguage.Id;
            someLanguage.CreatedDate = randomDateTime.AddMinutes(minutesInPast);
            var databaseUpdateException = new DbUpdateException();

            var failedLanguageException =
                new FailedLanguageStorageException(databaseUpdateException);

            var expectedLanguageDependencyException =
                new LanguageDependencyException(failedLanguageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectLanguageByIdAsync(languageId))
                    .ThrowsAsync(databaseUpdateException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Returns(randomDateTime);

            //when
            ValueTask<Language> modifyLanguageTask =
                this.languageService.ModifyLanguageAsync(someLanguage);

            LanguageDependencyException actualLanguageDependencyException =
                await Assert.ThrowsAsync<LanguageDependencyException>(
                    modifyLanguageTask.AsTask);

            //then
            actualLanguageDependencyException.Should().BeEquivalentTo(
                expectedLanguageDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLanguageByIdAsync(languageId), Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLanguageDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            //given
            int minutesInPast = GetRandomNegativeNumber();
            DateTimeOffset randomDateTime = GetRandomDatetimeOffset();
            Language randomLanguage = CreateRandomLanguage(randomDateTime);
            Language someLanguage = randomLanguage;
            someLanguage.CreatedDate = randomDateTime.AddMinutes(minutesInPast);
            Guid languageId = someLanguage.Id;
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedLanguageException =
                new LockedLanguageException(databaseUpdateConcurrencyException);

            var expectedLanguageDependencyValidationException =
                new LanguageDependencyValidationException(lockedLanguageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectLanguageByIdAsync(languageId))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Returns(randomDateTime);

            //when
            ValueTask<Language> modifyLanguageTask =
                this.languageService.ModifyLanguageAsync(someLanguage);

            LanguageDependencyValidationException actualLanguageDependencyValidationException =
                await Assert.ThrowsAsync<LanguageDependencyValidationException>(
                    modifyLanguageTask.AsTask);

            //then
            actualLanguageDependencyValidationException.Should().BeEquivalentTo(
                expectedLanguageDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLanguageByIdAsync(languageId), Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLanguageDependencyValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            //given
            int minutesInPast = GetRandomNegativeNumber();
            DateTimeOffset randomDateTime = GetRandomDatetimeOffset();
            Language randomLanguage = CreateRandomLanguage(randomDateTime);
            Language someLanguage = randomLanguage;
            someLanguage.CreatedDate = randomDateTime.AddMinutes(minutesInPast);
            var serviceException = new Exception();

            var failedLanguageException =
                new FailedLanguageServiceException(serviceException);

            var expectedLanguageServiceException =
                new LanguageServiceException(failedLanguageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectLanguageByIdAsync(someLanguage.Id))
                    .ThrowsAsync(serviceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Returns(randomDateTime);

            //when
            ValueTask<Language> modifyLanguage =
                this.languageService.ModifyLanguageAsync(someLanguage);

            LanguageServiceException actualLanguageServiceException =
                await Assert.ThrowsAsync<LanguageServiceException>(
                    modifyLanguage.AsTask);

            //then
            actualLanguageServiceException.Should().BeEquivalentTo(
                expectedLanguageServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLanguageByIdAsync(someLanguage.Id), Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLanguageServiceException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
