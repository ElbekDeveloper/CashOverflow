// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using CashOverflow.Brokers.DateTimes;
using CashOverflow.Brokers.Loggings;
using CashOverflow.Brokers.Storages;
using CashOverflow.Models.Companies;
using CashOverflow.Services.Foundations.Companies;
using Microsoft.Data.SqlClient;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace CashOverflow.Tests.Unit.Services.Foundations.Companies
{
    public partial class CompanyServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly ICompanyService companyService;

        public CompanyServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();

            this.companyService = new CompanyService(
                storageBroker: this.storageBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object);
        }

        public static TheoryData<int> InvalidMinutes()
        {
            int minutesInFuture = GetRandomNumber();
            int minutesInPast = GetRandomNegativeNumber();

            return new TheoryData<int>
            {
                minutesInFuture,
                minutesInPast
            };
        }

        private static int GetRandomNumber() =>
            new IntRange(min: 9, max: 99).GetValue();

        private static int GetRandomNegativeNumber() =>
            -1 * new IntRange(min: 9, max: 99).GetValue();

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: DateTime.UnixEpoch).GetValue();

        private static SqlException CreateSqlException() => 
            (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));

        private static Company CreateRandomCompany() =>
            CreateCompanyFiller(GetRandomDateTimeOffset()).Create();

        private static Company CreateRandomCompany(DateTimeOffset dates) =>
           CreateCompanyFiller(dates).Create();

        private static Filler<Company> CreateCompanyFiller(DateTimeOffset dates)
        {
            var filler = new Filler<Company>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dates);

            return filler;
        }
    }
}
