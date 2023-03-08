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
        private readonly ISalaryService salaryService;
        private readonly Mock<IStorageBroker> storageBrokerMock;
        public SalaryServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.salaryService = new SalaryService(
                storageBroker: storageBrokerMock.Object);
        }

        private Salary CreateRandomSalary()
            => CreateSalaryFiller(GetRandomDatetimeOffset()).Create();
        
        private Filler<Salary> CreateSalaryFiller(DateTimeOffset dates)
        {
            var filler = new Filler<Salary>();
            filler.Setup()
                .OnType<DateTimeOffset>().Use(dates);
            return filler;
        }
        private DateTimeOffset GetRandomDatetimeOffset()
            => new DateTimeRange(earliestDate: DateTime.UnixEpoch).GetValue();
    }
}
