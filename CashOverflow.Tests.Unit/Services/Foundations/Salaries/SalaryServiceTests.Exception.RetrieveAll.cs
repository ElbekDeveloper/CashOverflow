// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using CashOverflow.Models.Salaries.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace CashOverflow.Tests.Unit.Services.Foundations.Salaries
{
    public partial class SalaryServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = CreateSqlException();

            var faildeStorageException =
                new FailedSalaryStorageException(sqlException);

            var expectedSalaryDependencyException =
                new SalaryDependencyException(faildeStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllSalaries()).Throws(sqlException);

            // when
            Action retrieveAllSalarysAction = () =>
                this.salaryService.RetrieveAllSalaries();

            SalaryDependencyException actualSalaryDependencyException =
                Assert.Throws<SalaryDependencyException>(retrieveAllSalarysAction);

            // then
            actualSalaryDependencyException.Should().BeEquivalentTo(
                expectedSalaryDependencyException);

            this.storageBrokerMock.Verify(broker => broker.SelectAllSalaries(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedSalaryDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllWhenAllServiceErrorOccursAndLogIt()
        {
            // given
            string exceptionMessage = GetRandomString();
            var serviceException = new Exception(exceptionMessage);

            var failedSalaryServiceException =
                new FailedSalaryServiceException(serviceException);

            var expectedSalaryServiceException =
                new SalaryServiceException(failedSalaryServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllSalaries()).Throws(serviceException);

            // when
            Action retrieveAllSalaryAction = () =>
                this.salaryService.RetrieveAllSalaries();

            // then
            Assert.Throws<SalaryServiceException>(retrieveAllSalaryAction);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllSalaries(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSalaryServiceException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
