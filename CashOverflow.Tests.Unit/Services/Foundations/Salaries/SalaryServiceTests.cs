// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Linq.Expressions;
using CashOverflow.Brokers.Loggings;
using CashOverflow.Brokers.Storages;
using CashOverflow.Models.Salaries;
using CashOverflow.Services.Foundations.Salaries;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;

namespace CashOverflow.Tests.Unit.Services.Foundations.Salaries
{
    public partial class SalaryServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly ISalaryService salaryService;

        public SalaryServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.salaryService = new SalaryService(
                storageBroker: this.storageBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedExceptoin) =>
            actualException => actualException.SameExceptionAs(expectedExceptoin);
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
