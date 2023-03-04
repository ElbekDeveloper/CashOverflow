// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using CashOverflow.Brokers.Storages;
using CashOverflow.Models.Salaries;
using CashOverflow.Services.Foundations.Salaries;
using Moq;
using System;
using Tynamix.ObjectFiller;

namespace CashOverflow.Tests.Unit.Services.Foundations.Salaries
{
    public partial class SalaryServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly ISalaryService salaryService;

        public SalaryServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();

            this.salaryService = new SalaryService(
                storageBroker: this.storageBrokerMock.Object);
        }

        private DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: DateTime.UnixEpoch).GetValue();

        private Salary CreateRandomSalary() =>
            CreateSalaryFiller(dates: GetRandomDateTimeOffset()).Create();

        private Filler<Salary> CreateSalaryFiller(DateTimeOffset dates)
        {
            var filler = new Filler<Salary>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dates);

            return filler;
        }
    }
}
