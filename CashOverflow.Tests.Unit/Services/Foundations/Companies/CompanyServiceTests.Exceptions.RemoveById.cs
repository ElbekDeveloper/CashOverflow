// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Threading.Tasks;
using CashOverflow.Models.Companies;
using CashOverflow.Models.Companies.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace CashOverflow.Tests.Unit.Services.Foundations.Companies
{
    public partial class CompanyServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyOnRemoveIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            var databaseUpdateConcurencyException = new DbUpdateConcurrencyException();

            var lockedCompanyException = 
                new LockedCompanyException(databaseUpdateConcurencyException);

            var expectedCompanyDependencyException =
                new CompanyDependencyException(lockedCompanyException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCompanyByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurencyException);

            // when
            ValueTask<Company> removeByIdTask =
                this.companyService.RemoveCompanyById(someId);

            CompanyDependencyException actualCompanyDependencyException =
                await Assert.ThrowsAsync<CompanyDependencyException>(
                    removeByIdTask.AsTask);

            // then
            actualCompanyDependencyException.Should()
                .BeEquivalentTo(expectedCompanyDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCompanyByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCompanyDependencyException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteCompanyAsync(It.IsAny<Company>()), Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            SqlException sqlException = CreateSqlException();

            var failedCompanyStorageException =
                new FailedCompanyStorageException(sqlException);

            var expectedCompanyDependencyException =
                new CompanyDependencyException(failedCompanyStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCompanyByIdAsync(someId))
                    .Throws(sqlException);
            // when
            ValueTask<Company> removeCompanyTask =
                this.companyService.RemoveCompanyById(someId);

            CompanyDependencyException actualCompanyDependencyException =
                await Assert.ThrowsAsync<CompanyDependencyException>(
                    removeCompanyTask.AsTask);

            // then
            actualCompanyDependencyException.Should().BeEquivalentTo(expectedCompanyDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCompanyByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedCompanyDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
