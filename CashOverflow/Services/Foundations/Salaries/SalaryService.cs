// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using CashOverflow.Brokers.DateTimes;
using CashOverflow.Brokers.Loggings;
using CashOverflow.Brokers.Storages;
using CashOverflow.Models.Locations;
using CashOverflow.Models.Salaries;

namespace CashOverflow.Services.Foundations.Salaries
{
    public partial class SalaryService : ISalaryService
    {
        private IStorageBroker storageBroker;
        private ILoggingBroker loggingBroker;
        private IDateTimeBroker dateTimeBroker;

        public SalaryService(IStorageBroker storageBroker, ILoggingBroker loggingBroker, IDateTimeBroker dateTimeBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public ValueTask<Salary> AddSalaryAsync(Salary salary) =>
            throw new System.NotImplementedException();

        public IQueryable<Salary> RetrieveAllSalaries() =>
            TryCatch(() => this.storageBroker.SelectAllSalaries());
    }
}