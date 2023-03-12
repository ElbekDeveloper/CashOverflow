// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System.Threading.Tasks;
using CashOverflow.Models.Salaries;
using CashOverflow.Models.Salaries.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace CashOverflow.Tests.Unit.Services.Foundations.Salaries
{
    public partial class SalaryServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfInputIsNullAndLogItAsync()
        {
            //given
            Salary nullSalary = null;
            var nullSalaryException = new NullSalaryException();

            var expectedSalaryValidationException =
                new SalaryValidationException(nullSalaryException);

            //when
            ValueTask<Salary> addSalaryTask = this.salaryService.AddSalaryAsync(nullSalary);

            SalaryValidationException actualSalaryValidationException =
                await Assert.ThrowsAsync<SalaryValidationException>(addSalaryTask.AsTask);

            //then
            actualSalaryValidationException.Should().BeEquivalentTo(expectedSalaryValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedSalaryValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSalaryAsync(It.IsAny<Salary>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
