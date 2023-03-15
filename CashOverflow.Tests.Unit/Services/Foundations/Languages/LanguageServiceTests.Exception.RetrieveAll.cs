// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using CashOverflow.Models.Languages.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace CashOverflow.Tests.Unit.Services.Foundations.Languages
{
    public partial class LanguageServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = CreateSqlException();

            var failedStorageException =
                new FailedLanguageStorageException(sqlException);

            var expectedLanguageDependencyException =
                new LanguageDependencyException(failedStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllLanguages())
                    .Throws(sqlException);

            // when
            Action retrieveAllLanguagesAction = () =>
                this.languageService.RetrieveAllLanguages();

            LanguageDependencyException actualLanguageDependencyException =
                Assert.Throws<LanguageDependencyException>(
                    retrieveAllLanguagesAction);

            // then
            actualLanguageDependencyException.Should().BeEquivalentTo(
                expectedLanguageDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllLanguages(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedLanguageDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string exceptionMessage = GetRandomString();
            var serviceException = new Exception(exceptionMessage);

            var failedLanguageServiceException =
                new FailedLanguageServiceException(serviceException);

            var expectedLanguageServiceException =
                new LanguageServiceException(failedLanguageServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllLanguages()).Throws(serviceException);

            // when
            Action retrieveAllLanguagesAction = () =>
                this.languageService.RetrieveAllLanguages();

            LanguageServiceException actualLanguageServiceException =
                Assert.Throws<LanguageServiceException>(retrieveAllLanguagesAction);

            // then
            actualLanguageServiceException.Should().BeEquivalentTo(expectedLanguageServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllLanguages(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLanguageServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
