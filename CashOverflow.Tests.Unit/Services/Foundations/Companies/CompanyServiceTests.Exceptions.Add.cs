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
using Moq;
using Xunit;

namespace CashOverflow.Tests.Unit.Services.Foundations.Companies
{
    public partial class CompanyServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticialDependencyExceptionOnAddIfSqlExceptionOccursAndLogItAsync()
        {
            // given
            Company randomCompany = CreateRandomCompany();
            Company someCompany = randomCompany;
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
    }
}
