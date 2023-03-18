﻿// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System.Threading.Tasks;
using CashOverflow.Models.Salaries;
using CashOverflow.Models.Salaries.Exceptions;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace CashOverflow.Tests.Unit.Services.Foundations.Salaries
{
    public partial class SalaryServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfDepdencyErrorOccursAndLogItAsync()
        {
            // given
            Salary someSalary = CreateRandomSalary();
            SqlException sqlException = CreateSqlException();
            var faildSalaryStorageException = new FailedSalaryStorageException(sqlException);
            var expectedSalaryDependencyException = new SalaryDependencyException(faildSalaryStorageException);

            this.dateTimeBrokerMock.Setup(broker => broker.GetCurrentDateTimeOffset())
               .Throws(sqlException);

            // when
            ValueTask<Salary> addSalaryTask = this.salaryService.AddSalaryAsync(someSalary);

            SalaryDependencyException actualLocationDependencyException =
                await Assert.ThrowsAsync<SalaryDependencyException>(addSalaryTask.AsTask);

            // then
            actualLocationDependencyException.Should().BeEquivalentTo(expectedSalaryDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
            broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker => broker.LogCritical(It.Is(SameExceptionAs(expectedSalaryDependencyException))), Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfDuplicateKeyErrorOccursAndLogItAsync()
        {
            // given
            string someMessage = GetRandomString();
            Salary someSalary = CreateRandomSalary();
            var duplicateKeyException = new DuplicateKeyException(someMessage);
            var alreadyExistSalaryException = new AlreadyExistsSalaryException(duplicateKeyException);

            var expectedSalaryDependencyValidationException =
                new SalaryDependencyValidationException(alreadyExistSalaryException);

            this.dateTimeBrokerMock.Setup(broker => broker.GetCurrentDateTimeOffset())
                .Throws(duplicateKeyException);

            // when
            ValueTask<Salary> addSalaryTask = this.salaryService.AddSalaryAsync(someSalary);
            SalaryDependencyValidationException actualSalaryDependencyValidationException =
                await Assert.ThrowsAsync<SalaryDependencyValidationException>(addSalaryTask.AsTask);

            // then 
            actualSalaryDependencyValidationException.Should()
                .BeEquivalentTo(expectedSalaryDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker => broker.GetCurrentDateTimeOffset(), Times.Once);
            this.loggingBrokerMock.Verify(brokers => brokers.LogError(It.Is(SameExceptionAs(
                expectedSalaryDependencyValidationException))), Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}