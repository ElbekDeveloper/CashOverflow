// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System.Linq;
using System;
using CashOverflow.Brokers.Loggings;
using CashOverflow.Brokers.Storages;
using CashOverflow.Models.Jobs;
using CashOverflow.Services.Foundations.Jobs;
using Moq;
using Tynamix.ObjectFiller;

namespace CashOverflow.Tests.Unit.Services.Foundations.Jobs
{
    public partial class JobServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IJobService jobService;

        public JobServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.jobService = new JobService(
                storageBroker: this.storageBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private IQueryable<Job> CreateRandomJobs()
        {
            return CreateJobFiller(dates: GetRandomDateTime())
                .Create(count: GetRandomNumber()).AsQueryable();
        }

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: DateTime.UnixEpoch).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 99).GetValue();

        private static Filler<Job> CreateJobFiller(DateTimeOffset dates)
        {
            var filler = new Filler<Job>();

            filler.Setup().OnType<DateTimeOffset>().Use(dates);

            return filler;
        }
    }
}
