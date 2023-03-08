// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System.Threading.Tasks;
using CashOverflow.Brokers.Storages;
using CashOverflow.Models.Salaries;

namespace CashOverflow.Services.Foundations.Salaries
{
    public class SalaryService : ISalaryService
    {
        private IStorageBroker storageBroker;

        public SalaryService(IStorageBroker storageBroker) =>
            this.storageBroker = storageBroker;

        public async ValueTask<Salary> AddSalaryAsync(Salary salary) =>
            await this.storageBroker.InsertSalaryAsync(salary);
    }
}