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
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenSqlExceptionOccursAndLogItAsync()
        {
            //given
            Guid sameLanguageId = Guid.NewGuid();
            SqlException sqlException = CreateSqlException();

            var failedLanguageStorageException = new FailedLanguageStorageException(sqlException);

            var expectedLanguageDependencyException =
                new LanguageDependencyException(failedLanguageStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectLanguageByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            //when
            ValueTask<Language> removeLanguageByIdTask =
                this.languageService.RemoveLanguageByIdAsync(sameLanguageId);

            LanguageDependencyException actualLanguageDependencyException =
                await Assert.ThrowsAsync<LanguageDependencyException>(removeLanguageByIdTask.AsTask);

            //then
            actualLanguageDependencyException.Should()
                .BeEquivalentTo(expectedLanguageDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLanguageByIdAsync(It.IsAny<Guid>()), Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs
                    (expectedLanguageDependencyException))), Times.Once());

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnRemoveIFDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Guid someLanguageId = Guid.NewGuid();

            var databaseUpdateConcurrencyException =
                new DbUpdateConcurrencyException();

            var lockedLanguageException =
                new LockedLanguageException(databaseUpdateConcurrencyException);

            var expectedLanguageDependencyValidationException =
                new LanguageDependencyValidationException(lockedLanguageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectLanguageByIdAsync(It.IsAny<Guid>())).
                    ThrowsAsync(databaseUpdateConcurrencyException);
            // when
            ValueTask<Language> removeLanguageByIdTask =
                this.languageService.RemoveLanguageByIdAsync(someLanguageId);

            LanguageDependencyValidationException actualLanguageDependencyValidationException =
                await Assert.ThrowsAsync<LanguageDependencyValidationException>(
                    removeLanguageByIdTask.AsTask);
            // then

            actualLanguageDependencyValidationException.Should().BeEquivalentTo(
                expectedLanguageDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLanguageByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLanguageDependencyValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteLanguageAsync(It.IsAny<Language>()), Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveIfExceptionOccursAndLogItAsync()
        {
            // given
            Guid someLanguageId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedLanguageServiceException =
                new FailedLanguageServiceException(serviceException);

            var expectedLanguageServiceException =
                new LanguageServiceException(failedLanguageServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectLanguageByIdAsync(It.IsAny<Guid>())).
                    ThrowsAsync(serviceException);
            // when
            ValueTask<Language> removeLanguageByIdTask =
                this.languageService.RemoveLanguageByIdAsync(someLanguageId);

            LanguageServiceException actualLanguageServiceException =
                await Assert.ThrowsAsync<LanguageServiceException>(
                    removeLanguageByIdTask.AsTask);

            // then
            actualLanguageServiceException.Should().BeEquivalentTo(
                expectedLanguageServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLanguageByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLanguageServiceException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
