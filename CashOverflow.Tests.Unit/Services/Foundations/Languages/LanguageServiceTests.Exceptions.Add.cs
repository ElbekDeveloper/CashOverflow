// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System.Threading.Tasks;
using CashOverflow.Models.Languages;
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
    }
}
