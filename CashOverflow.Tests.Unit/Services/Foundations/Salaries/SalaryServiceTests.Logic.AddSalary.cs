// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using CashOverflow.Models.Salaries;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace CashOverflow.Tests.Unit.Services.Foundations.Salaries
{
    public partial class SalaryServiceTests
    {
        [Fact]
        public async Task ShouldAddSalaryAsync()
        {
            //given
            Salary randomSalary = CreateRandomSalary();
            Salary inputSalary = randomSalary;
            Salary persistedSalary = inputSalary;
            Salary expectedSalary = persistedSalary.DeepClone();
            
            this.storageBrokerMock.Setup(broker => 
            broker.InsertSalaryAsync(inputSalary)).ReturnsAsync(expectedSalary);

            //when
            Salary actualSalary = await this.salaryService
                .AddSalaryAsync(inputSalary);

            //then
            actualSalary.Should().BeEquivalentTo(expectedSalary);
            
            this.storageBrokerMock.Verify(broker =>
            broker.InsertSalaryAsync(inputSalary), Times.Once());

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
