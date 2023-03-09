using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashOverflow.Models.Salaries;
using FluentAssertions;
using Moq;
using Xunit;

namespace CashOverflow.Tests.Unit.Services.Foundations.Salaries
{
    public partial class SalaryServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveAllSalarys()
        {
            //given
            IQueryable<Salary> randomSalaries = CreateRandomSalaries();
            IQueryable<Salary> storageSalaries = randomSalaries;
            IQueryable<Salary> expectedSalaries = storageSalaries;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllSalaries()).Returns(expectedSalaries);

            //when
            IQueryable<Salary> actualSalaries = this.salaryService.RetrieveAllSalary();

            //then
            actualSalaries.Should().BeEquivalentTo(expectedSalaries);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllSalaries(), Times.Once());

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
