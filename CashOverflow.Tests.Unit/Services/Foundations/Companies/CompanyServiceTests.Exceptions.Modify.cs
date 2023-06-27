// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Threading.Tasks;
using CashOverflow.Models.Companies;
using CashOverflow.Models.Companies.Exceptions;
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
            DateTimeOffset someDateTime = GetRandomDateTime();
            Company randomCompany = CreateRandomCompany();
            Company someCompany = randomCompany;
            Guid companyId = someCompany.Id;
            SqlException sqlException = CreateSqlException();

            var failedCompanyStorageException = 
                new FailedCompanyStorageException(sqlException);
            
            var expectedCompanyDepdencyException = 
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
            this.storageBrokerMock.Verify(broker => 
                broker.SelectCompanyByIdAsync(companyId), Times.Once);
            
            this.storageBrokerMock.Verify(broker => 
                broker.UpdateCompanyAsync(someCompany), Times.Never);
            
            this.storageBrokerMock.VerifyNoOtherCalls();;
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

    }
}