// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CashOverflow.Brokers.Storages;
using CashOverflow.Models.Jobs;

namespace CashOverflow.Services.Foundations.Jobs
{
    public class JobService: IJobService
    {
        private readonly IStorageBroker storageBroker;
        public JobService(IStorageBroker storageBroker)
        {
            this.storageBroker = storageBroker;
        }

        public IQueryable<Job> RetrieveAllJobs() =>
            this.storageBroker.SelectAllJobs();
    }
}