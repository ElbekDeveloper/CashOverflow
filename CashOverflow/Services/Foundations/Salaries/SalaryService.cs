// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using CashOverflow.Brokers.Storages;
using CashOverflow.Models.Salaries;
using System.Threading.Tasks;

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