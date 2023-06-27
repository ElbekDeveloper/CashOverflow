// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Threading.Tasks;
using CashOverflow.Models.Companies;
using CashOverflow.Models.Companies.Exceptions;
using FluentAssertions;
using Force.DeepCloner;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace CashOverflow.Tests.Unit.Services.Foundations.Companies
{
    public partial class CompanyServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given 
            Company randomCompany = CreateRandomCompany();
            Company someCompany = randomCompany;
            Guid companyId = someCompany.Id;
            SqlException sqlException = CreateSqlException();

            var failedCompanyStorageException = 
                new FailedCompanyStorageException(sqlException);
            
            var expectedCompanyDependencyException = 
                new CompanyDependencyException(failedCompanyStorageException);

            this.storageBrokerMock.Setup(broker => 
                broker.SelectCompanyByIdAsync(companyId))
                    .ThrowsAsync(sqlException);
            
            // when 
            ValueTask<Company> modifyCompanyTask =
                this.companyService.ModifyCompanyAsync(someCompany);

            CompanyDependencyException actualCompanyDependencyException =
                await Assert.ThrowsAsync<CompanyDependencyException>(
                    modifyCompanyTask.AsTask);

            // then
            actualCompanyDependencyException.Should().BeEquivalentTo(expectedCompanyDependencyException);
            
            this.storageBrokerMock.Verify(broker => 
                broker.SelectCompanyByIdAsync(companyId), Times.Once);
            
            this.loggingBrokerMock.Verify(broker => 
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedCompanyDependencyException))));
            
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
        
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given 
            Company randomCompany = CreateRandomCompany();
            Company someCompany = randomCompany;
            Company storageCompany = someCompany.DeepClone();
            Guid companyId = someCompany.Id;
            var databaseUpdateException = new DbUpdateException();
        
            var failedStorageCompanyException =
                new FailedCompanyStorageException(databaseUpdateException);
        
            var expectedCompanyDependencyException = 
                new CompanyDependencyException(failedStorageCompanyException);

            this.storageBrokerMock.Setup(broker => 
                broker.SelectCompanyByIdAsync(companyId))
                    .ThrowsAsync(databaseUpdateException);

            // when 
            ValueTask<Company> modifyCompanyTask =
                this.companyService.ModifyCompanyAsync(someCompany);

            CompanyDependencyException actualCompanyDependencyException =
                await Assert.ThrowsAsync<CompanyDependencyException>(modifyCompanyTask.AsTask);
            
            // then
            actualCompanyDependencyException.Should().BeEquivalentTo(expectedCompanyDependencyException);
            
            this.storageBrokerMock.Verify(broker => 
                broker.SelectCompanyByIdAsync(companyId), Times.Once);
            
            this.loggingBrokerMock.Verify(broker => 
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedCompanyDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
        
        [Fact]
        public async Task
            ShouldThrowDependencyValidationExceptionOnModifyIfDatabaseUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given 
            Company randomCompany = CreateRandomCompany();
            Company someCompany = randomCompany;
            Guid companyId = someCompany.Id;
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var failedCompanyStorageException = 
                new FailedCompanyStorageException(databaseUpdateConcurrencyException);

            var expectedCompanyDependencyException =
                new CompanyDependencyException(failedCompanyStorageException);

            this.storageBrokerMock.Setup(broker => 
                    broker.SelectCompanyByIdAsync(companyId)).ThrowsAsync(databaseUpdateConcurrencyException);

            // when 
            ValueTask<Company> modifyCompanyTask = 
                this.companyService.ModifyCompanyAsync(someCompany);

            CompanyDependencyException actualCompanyDependencyException =
                await Assert.ThrowsAsync<CompanyDependencyException>(modifyCompanyTask.AsTask);

            // then
            actualCompanyDependencyException.Should()
                .BeEquivalentTo(expectedCompanyDependencyException);
            
            this.storageBrokerMock.Verify(broker => 
                broker.SelectCompanyByIdAsync(companyId), Times.Once);

            this.loggingBrokerMock.Verify(broker => 
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedCompanyDependencyException))));
            
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

    }
}