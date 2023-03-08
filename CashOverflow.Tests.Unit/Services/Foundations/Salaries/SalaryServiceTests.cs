// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using CashOverflow.Brokers.Loggings;
using CashOverflow.Brokers.Storages;
using CashOverflow.Models.Salaries;
using CashOverflow.Services.Foundations.Salaries;
using Moq;
using System;
using System.Linq;
using Tynamix.ObjectFiller;

namespace CashOverflow.Tests.Unit.Services.Foundations.Salaries
{
    public partial class SalaryServiceTests
    {
        private readonly ISalaryService salaryService;
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        public SalaryServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.salaryService = new SalaryService(
                storageBroker: storageBrokerMock.Object,
                loggingBroker: loggingBrokerMock.Object);
        }

        private Salary CreateRandomSalary()
            => CreateSalaryFiller(GetRandomDatetimeOffset()).Create();

        private IQueryable<Salary> CreateRandomSalaries() =>
            CreateSalaryFiller(dates: GetRandomDatetimeOffset()).Create(count: GetRandomNumber()).AsQueryable();


        private Filler<Salary> CreateSalaryFiller(DateTimeOffset dates)
        {
            var filler = new Filler<Salary>();
            filler.Setup()
                .OnType<DateTimeOffset>().Use(dates);
            return filler;
        }
        private DateTimeOffset GetRandomDatetimeOffset()
            => new DateTimeRange(earliestDate: DateTime.UnixEpoch).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 99).GetValue();
    }
}
