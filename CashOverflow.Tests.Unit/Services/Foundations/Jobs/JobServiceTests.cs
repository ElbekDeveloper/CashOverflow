// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using CashOverflow.Brokers.Storages;
using CashOverflow.Models.Jobs;
using CashOverflow.Services.Foundations.Jobs;
using Moq;
using System;
using Tynamix.ObjectFiller;

namespace CashOverflow.Tests.Unit.Services.Foundations.Jobs
{
    public partial class JobServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private IJobService jobService;
        public JobServiceTests()
        {
            this.storageBrokerMock= new Mock<IStorageBroker>();
            this.jobService=new JobService(this.storageBrokerMock.Object);

        }

        private DateTimeOffset GetRandomDatetimeOffSet()=>
            new DateTimeRange(earliestDate: DateTime.UnixEpoch).GetValue();
        private Job CreateRandomJob() => CreateJobFiller(GetRandomDatetimeOffSet()).Create();

        private Filler<Job> CreateJobFiller(DateTimeOffset dates)
        {
            var filler = new Filler<Job>();

            filler.Setup().OnType<DateTimeOffset>().Use(dates);
            
            return filler;
        }
    }
}
