// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Linq.Expressions;
using CashOverflow.Brokers.DateTimes;
using CashOverflow.Brokers.Loggings;
using CashOverflow.Brokers.Storages;
using CashOverflow.Models.Jobs;
using CashOverflow.Services.Foundations.Jobs;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;

namespace CashOverflow.Tests.Unit.Services.Foundations.Jobs
{
	public partial class JobServiceTests
	{
		private readonly Mock<IStorageBroker> storageBrokerMock;
		private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
		private readonly Mock<ILoggingBroker> loggingBrokerMock;
		private IJobService jobService;

		public JobServiceTests()
		{
			this.storageBrokerMock = new Mock<IStorageBroker>();
			this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
			this.loggingBrokerMock = new Mock<ILoggingBroker>();

			this.jobService = new JobService(
				storageBroker: this.storageBrokerMock.Object,
				dateTimeBroker:this.dateTimeBrokerMock.Object,
				loggingBroker: this.loggingBrokerMock.Object);

		}

		private Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
			actualException => actualException.SameExceptionAs(expectedException);

		private DateTimeOffset GetRandomDatetimeOffset() =>
			new DateTimeRange(earliestDate: DateTime.UnixEpoch).GetValue();

		private Job CreateRandomJob() =>
			CreateJobFiller(dates: GetRandomDatetimeOffset()).Create();

		private Filler<Job> CreateJobFiller(DateTimeOffset dates)
		{
			var filler = new Filler<Job>();

			filler.Setup().OnType<DateTimeOffset>().Use(dates);

			return filler;
		}
	}
}

