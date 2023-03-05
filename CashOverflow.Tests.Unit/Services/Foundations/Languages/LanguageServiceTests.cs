// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using CashOverflow.Brokers.Storages;
using CashOverflow.Brokers.Loggings;
using Moq;
using CashOverflow.Services.Foundations.Languages;
using System.Linq;
using CashOverflow.Models.Languages;
using System;
using Tynamix.ObjectFiller;
using Microsoft.Data.SqlClient;
using System.Runtime.Serialization;
using System.Linq.Expressions;
using Xeptions;

namespace CashOverflow.Tests.Unit.Services.Foundations.Languages
{
    public partial class LanguageServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly ILanguageService languageService;

        public LanguageServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.languageService = new LanguageService(
                storageBroker: this.storageBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static IQueryable<Language> CreateRandomLanguages()
        {
            return CreateLanguageFiller(date: GetRandomDateTimeOffset())
                .Create(count: GetRandomNumber())
                    .AsQueryable();
        }

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static SqlException GetSqlException() =>
            (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static Filler<Language> CreateLanguageFiller(DateTimeOffset date)
        {
            var filler = new Filler<Language>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(date);

            return filler;
        }
    }
}
