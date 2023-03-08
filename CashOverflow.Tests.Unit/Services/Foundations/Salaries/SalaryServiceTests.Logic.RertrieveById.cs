// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using CashOverflow.Models.Salaries;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CashOverflow.Tests.Unit.Services.Foundations.Salaries
{
    public partial class SalaryServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveSalaryByIdAsync()
        {
            //given
            Guid randomSalaryId = Guid.NewGuid();
            Guid inputSalaryId = randomSalaryId;
            Salary randomSalary = CreateRandomSalary();
            Salary storageSalary = randomSalary;
            Salary expectedSalary = storageSalary.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSalaryByIdAsync(inputSalaryId))
                    .ReturnsAsync(expectedSalary);

            //when
            Salary actualSalary = await this.salaryService
                    .RetriveSalaryByIdAsync(inputSalaryId);

            //then
            actualSalary.Should().BeEquivalentTo(expectedSalary);

            this.storageBrokerMock.Verify(broker =>
            broker.SelectSalaryByIdAsync(inputSalaryId), Times.Once());

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
