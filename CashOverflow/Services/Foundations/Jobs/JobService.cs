// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CashOverflow.Brokers.Loggings;
using CashOverflow.Brokers.Storages;
using CashOverflow.Models.Jobs;

namespace CashOverflow.Services.Foundations.Jobs
{
    public partial class JobService: IJobService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;
        public JobService
        (
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker
        )
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
        }
        public IQueryable<Job> RetrieveAllJobs() =>
            TryCatch(() =>
                {
                    return this.storageBroker.SelectAllJobs();
                });
    }
}