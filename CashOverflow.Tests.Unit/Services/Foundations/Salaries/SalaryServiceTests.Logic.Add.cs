// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Threading.Tasks;
using CashOverflow.Models.Salaries;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Xunit;

namespace CashOverflow.Tests.Unit.Services.Foundations.Salaries
{
    public partial class SalaryServiceTests
    {
        [Fact]
        public async Task ShouldAddSalaryAsync()
        {
            // given 
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            Salary randomSalary = CreateRandomSalary(randomDateTime);
            Salary inputSalary = randomSalary;
            Salary persistedSalary = inputSalary;
            Salary expectedSalary = persistedSalary.DeepClone();

            this.dateTimeBrokerMock.Setup(broker => broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertSalaryAsync(inputSalary)).ReturnsAsync(persistedSalary);

            // when
            Salary actualSalary = await this.salaryService.AddSalaryAsync(inputSalary);

            // then
            actualSalary.Should().BeEquivalentTo(expectedSalary);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSalaryAsync(inputSalary), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}