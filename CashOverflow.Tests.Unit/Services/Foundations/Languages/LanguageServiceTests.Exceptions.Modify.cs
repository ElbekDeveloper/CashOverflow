using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        }

    }
}
