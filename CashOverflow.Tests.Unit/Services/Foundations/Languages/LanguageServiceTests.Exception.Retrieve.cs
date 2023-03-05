// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using CashOverflow.Models.Languages.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using System;
using Xunit;

namespace CashOverflow.Tests.Unit.Services.Foundations.Languages
{
    public partial class LanguageServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

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
    }
}
