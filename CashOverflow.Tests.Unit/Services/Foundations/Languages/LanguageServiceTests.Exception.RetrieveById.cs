using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
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
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdAsyncIfSqlErrorOccursAndLogItAsync()
        {
            //given
            Guid someId= Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedLanguageStorageException = 
                new FailedLanguageStorageException(sqlException);

            LanguageDependencyException expectedLanguageDependencyException =
                new LanguageDependencyException(failedLanguageStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectLanguageByIdAsync(It.IsAny<Guid>())).ThrowsAsync(sqlException);

            //when
            ValueTask<Language> retrieveLanguageByIdTask=
                this.languageService.RetrieveLanguageByIdAsync(someId);

            LanguageDependencyException actualLanguageDependencyException =
                await Assert.ThrowsAsync<LanguageDependencyException>(retrieveLanguageByIdTask.AsTask);

            //then
            actualLanguageDependencyException.Should().BeEquivalentTo(expectedLanguageDependencyException);

            this.storageBrokerMock.Verify(broker=>
                broker.SelectLanguageByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedLanguageDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();

        }

    }
}
