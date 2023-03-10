// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CashOverflow.Brokers.Loggings;
using CashOverflow.Brokers.Storages;
using CashOverflow.Models.Jobs;
using CashOverflow.Models.Jobs.Exceptions;
using Xeptions;

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

        private Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);


        public ValueTask<Job> AddJobAsync(Job job) =>
      throw new NotImplementedException();


    }
}
