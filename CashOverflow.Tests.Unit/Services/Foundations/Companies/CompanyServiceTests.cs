// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using CashOverflow.Brokers.DateTimes;
using CashOverflow.Brokers.Loggings;
using CashOverflow.Brokers.Storages;
using CashOverflow.Models.Companies;
using CashOverflow.Services.Foundations.Companies;
using Moq;
using Tynamix.ObjectFiller;

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

        private static Company CreateRandomCompany() =>
            CreateCompanyFiller(GetRandomDateTime()).Create();

        private static Company CreateRandomCompany(DateTimeOffset date) =>
            CreateCompanyFiller(date).Create();


        private static Filler<Company> CreateCompanyFiller(DateTimeOffset dates)
        {
            var filler = new Filler<Company>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dates);

            return filler;
        }
    }
}
