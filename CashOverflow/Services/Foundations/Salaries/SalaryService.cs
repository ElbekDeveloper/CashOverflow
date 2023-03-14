// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System.Threading.Tasks;
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

        public ValueTask<Salary> AddSalaryAsync(Salary salary) =>
        TryCatch(async () =>
        {
            ValidateSalaryOnAdd(salary);

            return await this.storageBroker.InsertSalaryAsync(salary);
        });
    }
}