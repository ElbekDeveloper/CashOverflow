// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using CashOverflow.Services.Foundations.Jobs;
using CashOverflow.Brokers.Storages;
using CashOverflow.Models.Jobs;
using Tynamix.ObjectFiller;

namespace CashOverflow.Tests.Unit.Services.Foundations.Jobs
{
    public partial class JobServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly IJobService jobService;
        public JobServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.jobService = new JobService(
                storageBroker: this.storageBrokerMock.Object);
        }

        private static IQueryable<Job> CreateRandomJobs()
        {
            return CreateJobFiller(dates: GetRandomDateTimeOffset())
                .Create(count: GetRandomNumber())
                    .AsQueryable();
        }

        private static int GetRandomNumber()=>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset()=>
            new DateTimeRange(earliestDate: DateTime.UnixEpoch).GetValue();

        private static Filler<Job> CreateJobFiller(DateTimeOffset dates)
        {
            var filler = new Filler<Job>();
            
            filler.Setup()
                .OnType<DateTimeOffset>().Use(dates);
            
            return filler;
        }
    }
}