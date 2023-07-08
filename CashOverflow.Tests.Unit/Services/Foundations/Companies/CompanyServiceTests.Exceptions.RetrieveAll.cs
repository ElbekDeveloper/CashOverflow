// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using CashOverflow.Models.Companies.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using System;
using Xunit;

namespace CashOverflow.Tests.Unit.Services.Foundations.Companies
{
    public partial class CompanyServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = CreateSqlException();
            var failedCompanyServiceException = new FailedCompanyServiceException(sqlException);

            var expectedCompanyDependencyException =
                new CompanyDependencyException(failedCompanyServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllCompanies()).Throws(sqlException);

            // when
            Action retrieveAllCompanyAAction = () =>
                this.companyService.RetrieveAllCompanies();

            CompanyDependencyException actualCompanyDependencyException =
                Assert.Throws<CompanyDependencyException>(retrieveAllCompanyAAction);

            // then
            actualCompanyDependencyException.Should().BeEquivalentTo(expectedCompanyDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllCompanies(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedCompanyDependencyException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllWhenAllServicesErrorOccursAndLogIt()
        {
            // given
            string exceptionMessage = GetRandomString();
            var serviceException = new Exception(exceptionMessage);
            var failedCompanyServiceException = new FailedCompanyServiceException(serviceException);

            var expectedCompanyServiceException =
                new CompanyServiceException(failedCompanyServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllCompanies()).Throws(serviceException);

            // when
            Action retrieveAllCompanyAction = () =>
                this.companyService.RetrieveAllCompanies();

            CompanyServiceException actualCompanyServiceException =
                Assert.Throws<CompanyServiceException>(retrieveAllCompanyAction);

            // then
            actualCompanyServiceException.Should().BeEquivalentTo(expectedCompanyServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllCompanies(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCompanyServiceException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
