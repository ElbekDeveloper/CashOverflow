// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using CashOverflow.Models.Salaries;
using FluentAssertions;
using Moq;
using System.Linq;
using System.Threading.Tasks;
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
