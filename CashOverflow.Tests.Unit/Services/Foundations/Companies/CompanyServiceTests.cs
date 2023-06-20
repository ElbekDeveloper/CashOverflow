// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Runtime.InteropServices;
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
        private readonly Mock<ILoggingBroker> loggingBroker;
        private readonly ICompanyService companyService;

        public CompanyServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBroker = new Mock<ILoggingBroker>();

			this.companyService = new CompanyService(
                storageBroker: this.storageBrokerMock.Object,
				loggingBroker: this.loggingBroker.Object);
        }

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: DateTime.UnixEpoch).GetValue();

        private static Company CreateRandomCompany() =>
            CreateCompanyFiller().Create();

        private static Filler<Company> CreateCompanyFiller()
        {
            var filler = new Filler<Company>();
            DateTimeOffset dates = GetRandomDateTimeOffset();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dates);

            return filler;
        }

        
    }
}
