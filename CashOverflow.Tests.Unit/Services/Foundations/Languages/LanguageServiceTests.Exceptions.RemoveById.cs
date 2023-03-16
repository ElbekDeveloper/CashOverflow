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
using Moq;
using Xunit;

namespace CashOverflow.Tests.Unit.Services.Foundations.Languages {
    public partial class LanguageServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenSqlExceptionOccursAndLogItAsync()
        {
            //given
            Guid sameLanguageId= Guid.NewGuid();
            SqlException sqlException = CreateSqlException();

            var failedLanguageStorageException=new FailedLanguageStorageException(sqlException);

            var expectedLanguageDependencyException =
                new LanguageDependencyException(failedLanguageStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectLanguageByIdAsync(It.IsAny<Guid>()))
                .ThrowsAsync(sqlException);

            //when
            ValueTask<Language> removeLanguageByIdTask=
                this.languageService.RemoveLanguageByIdAsync(sameLanguageId);

            LanguageDependencyException actualLanguageDependencyException =
                await Assert.ThrowsAsync<LanguageDependencyException>(removeLanguageByIdTask.AsTask);

            //then
            actualLanguageDependencyException.Should()
                .BeEquivalentTo(expectedLanguageDependencyException);

            this.storageBrokerMock.Verify(broker=>
                broker.SelectLanguageByIdAsync(It.IsAny<Guid>()), Times.Once());

            this.loggingBrokerMock.Verify(broker=>
                broker.LogCritical(It.Is(SameExceptionAs
                (expectedLanguageDependencyException))), Times.Once()); 

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
