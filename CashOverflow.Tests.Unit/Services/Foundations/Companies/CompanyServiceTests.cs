// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using CashOverflow.Brokers.DateTimes;
using CashOverflow.Brokers.Loggings;
using CashOverflow.Brokers.Storages;
using CashOverflow.Models.Companies;
using CashOverflow.Services.Foundations.Companies;
using Moq;
using System;
using System.Linq.Expressions;
using Tynamix.ObjectFiller;
using Xeptions;

namespace CashOverflow.Tests.Unit.Services.Foundations.Companies
{
    public partial class CompanyServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly ICompanyService companyService;

        public CompanyServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.companyService = new CompanyService(
                storageBroker: this.storageBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: DateTime.UnixEpoch).GetValue();

        private static Expression<Func<Exception, bool>> SameExceptionAs(Xeption expectedException)
            => actualException => actualException.SameExceptionAs(expectedException);

        private static int GetRandomNegativeNumber() =>
            -1 * new IntRange(min: 2, max: 10).GetValue();

        private static Company CreateRandomModifyCompany(DateTimeOffset dates)
        {
            int randomDaysInPast = GetRandomNegativeNumber();
            Company randomCompany = CreateRandomCompany(dates);

            randomCompany.CreatedDate = randomCompany.CreatedDate.AddDays(randomDaysInPast);

            return randomCompany;
        }

        private static Company CreateRandomCompany(DateTimeOffset dates) =>
           CreateCompanyFiller(dates).Create();

        private static Company CreateRandomCompany() =>
            CreateCompanyFiller(GetRandomDateTime()).Create();

        private static Filler<Company> CreateCompanyFiller(DateTimeOffset dates)
        {
            var filler = new Filler<Company>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dates);

            return filler;
        }
    }
}
