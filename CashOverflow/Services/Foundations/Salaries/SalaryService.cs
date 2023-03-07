using CashOverflow.Brokers.Storages;
using CashOverflow.Models.Salaries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashOverflow.Services.Foundations.Salaries
{
    public class SalaryService : ISalaryService
    {
        private readonly IStorageBroker storageBroker;

        public SalaryService(IStorageBroker storageBroker)
        {
            this.storageBroker = storageBroker;
        }

        public async ValueTask<Salary> AddSalaryAsync(Salary salary) =>
            await storageBroker.InsertSalaryAsync(salary);

        public IQueryable<Salary> RetrieveSalaryAll() =>
            storageBroker.SelectAllSalaries();
        

        public ValueTask<Salary> RetriveSalaryByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
