// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Threading.Tasks;
using CashOverflow.Brokers.Loggings;
using CashOverflow.Brokers.Storages;
using CashOverflow.Models.Jobs;
using CashOverflow.Models.Jobs.Exceptions;

namespace CashOverflow.Services.Foundations.Jobs
{
	public partial class JobService:IJobService
	{
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;

        public JobService(IStorageBroker storageBroker,ILoggingBroker loggingBroker)
		{
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
		}

        public ValueTask<Job> AddJobAsync(Job job) =>
        TryCatch(async () =>
        {
            ValidateJobNotNull(job);

            return await this.storageBroker.InsertJobAsync(job);
        });


    }
}

