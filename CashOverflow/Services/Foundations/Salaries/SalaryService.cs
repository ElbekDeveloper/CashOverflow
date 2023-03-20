// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System.Linq;
using CashOverflow.Brokers.Loggings;
using CashOverflow.Brokers.Storages;
using CashOverflow.Models.Salaries;

namespace CashOverflow.Services.Foundations.Salaries
{
    public partial class SalaryService : ISalaryService
    {
        private IStorageBroker storageBroker;
        private ILoggingBroker loggingBroker;

        public SalaryService(IStorageBroker storageBroker, ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
        }

        public IQueryable<Salary> RetrieveAllSalaries() =>
            TryCatch(() => this.storageBroker.SelectAllSalaries());
    }
}