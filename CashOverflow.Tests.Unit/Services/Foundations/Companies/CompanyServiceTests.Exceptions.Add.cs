// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Threading.Tasks;
using CashOverflow.Models.Companies;
using CashOverflow.Models.Companies.Exceptions;
using EFxceptions.Models.Exceptions;
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
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlExceptionOccursAndLogItAsync()
        {
            // given
            Company someCompany = CreateRandomCompany();
            Guid companyId = someCompany.Id;
            SqlException sqlException = CreateSqlException();

            FailedCompanyStorageException failedCompanyStorageException = 
                new FailedCompanyStorageException(sqlException);

            CompanyDependencyException expectedCompanyDependencyException = 
                new CompanyDependencyException(failedCompanyStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask<Company> addCompanyTask = this.companyService
                .AddCompanyAsync(someCompany);

            CompanyDependencyException actualCompanyDependencyException =
                await Assert.ThrowsAsync<CompanyDependencyException>(addCompanyTask.AsTask);

            // then
            actualCompanyDependencyException.Should().BeEquivalentTo(expectedCompanyDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker => 
                broker.LogCritical(It.Is(SameExceptionAs(expectedCompanyDependencyException))), Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfDuplicateKeyErrorOccurredAndLogItAsync()
        {
            // given
            Company someCompany = CreateRandomCompany();
            string someMessage = GetRandomString();
            var duplicateKeyException = new DuplicateKeyException(someMessage);

            var alreadyExistsCompanyException = 
                new AlreadyExistsCompanyException(duplicateKeyException);

            var expectedCompanyDependencyValidationException =
                new CompanyDependencyValidationException(alreadyExistsCompanyException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Throws(duplicateKeyException);

            // when
            ValueTask<Company> addCompanyTask = this.companyService.AddCompanyAsync(someCompany);

            CompanyDependencyValidationException actualCompanyDependencyValidationException =
                await Assert.ThrowsAsync<CompanyDependencyValidationException>(
                    addCompanyTask.AsTask);

            // then
            actualCompanyDependencyValidationException.Should().BeEquivalentTo(
                expectedCompanyDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCompanyDependencyValidationException))), Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddIfDbUpdateConcurrencyErrorOccurredAndLogItAsync()
        {
            // given
            Company someCompany = CreateRandomCompany();
            string someMessage = GetRandomString();

            var dbUpdateConcurrencyException = 
                new DbUpdateConcurrencyException(someMessage);

            var lockedCompanyException = 
                new LockedCompanyException(dbUpdateConcurrencyException);

            var expectedCompanyDependencyException = 
                new CompanyDependencyException(lockedCompanyException);

            this.dateTimeBrokerMock.Setup(broker => 
                broker.GetCurrentDateTimeOffset())
                    .Throws(dbUpdateConcurrencyException);

            // when
            ValueTask<Company> addCompanyTask = 
                this.companyService.AddCompanyAsync(someCompany);

            CompanyDependencyException actualCompanyDependencyException =
                await Assert.ThrowsAsync<CompanyDependencyException>(addCompanyTask.AsTask);

            // then
            actualCompanyDependencyException.Should().BeEquivalentTo(expectedCompanyDependencyException);

            this.dateTimeBrokerMock.Verify(broker => 
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCompanyDependencyException))), Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
        
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddIfDbUpdateExceptionOccuredAndLogItAsync()
        {
            // given
            Company someCompany = CreateRandomCompany();
            string someMessage = GetRandomString();

            var dbUpdateException = 
                new DbUpdateException(someMessage);

            var failedCompanyStorageException = 
                new FailedCompanyStorageException(dbUpdateException);

            var expectedCompanyDependencyException =
                new CompanyDependencyException(failedCompanyStorageException);

            this.dateTimeBrokerMock.Setup(broker => 
                    broker.GetCurrentDateTimeOffset()).Throws(dbUpdateException);

            // when
            ValueTask<Company> addCompanyTask = 
                this.companyService.AddCompanyAsync(someCompany);

            CompanyDependencyException actualCompanyDependencyException = 
                await Assert.ThrowsAsync<CompanyDependencyException>(addCompanyTask.AsTask);

            // then
            actualCompanyDependencyException.Should().BeEquivalentTo(expectedCompanyDependencyException);
            
            this.dateTimeBrokerMock.Verify(broker => 
                broker.GetCurrentDateTimeOffset(), Times.Once);
            
            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCompanyDependencyException))), Times.Once);
            
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
