// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using CashOverflow.Brokers.Storages;
using CashOverflow.Models.Salaries;
using System.Threading.Tasks;

namespace CashOverflow.Services.Foundations.Salarys
{
    public class SalaryService : ISalaryService
    {
        private readonly IStorageBroker storageBroker;

        public SalaryService(IStorageBroker storageBroker)
        {
            this.storageBroker = storageBroker;
        }

        public async ValueTask<Salary> AddSalaryAsync(Salary salary) =>
            return await storageBroker.AddSalaryAsync(salary);
    }
}
