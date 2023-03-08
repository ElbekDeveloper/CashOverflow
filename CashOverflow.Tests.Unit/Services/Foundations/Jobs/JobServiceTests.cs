// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------
using System.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using CashOverflow.Services.Foundations.Jobs;
using CashOverflow.Brokers.Storages;
using CashOverflow.Brokers.DateTimes;
using CashOverflow.Models.Jobs;
using Tynamix.ObjectFiller;
using Microsoft.Data.SqlClient;
using System.Runtime.Serialization;
using CashOverflow.Brokers.Loggings;
using Xeptions;

namespace CashOverflow.Tests.Unit.Services.Foundations.Jobs
{
    public partial class JobServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly IJobService jobService;
        public JobServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();

            this.jobService = new JobService(
                storageBroker: this.storageBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object);
        }

        private static IQueryable<Job> CreateRandomJobs()
        {
            return CreateJobFiller(dates: GetRandomDateTimeOffset())
                .Create(count: GetRandomNumber())
                    .AsQueryable();
        }

        private static int GetRandomNumber()=>
            new IntRange(min: 2, max: 10).GetValue();
        
        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset()=>
            new DateTimeRange(earliestDate: DateTime.UnixEpoch).GetValue();

        private static Filler<Job> CreateJobFiller(DateTimeOffset dates)
        {
            var filler = new Filler<Job>();
            
            filler.Setup()
                .OnType<DateTimeOffset>().Use(dates);
            
            return filler;
        }
        private static SqlException CreateSqlException() =>
            (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));

        private Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);
    }
}