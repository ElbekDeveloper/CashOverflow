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
            throw new NotImplementedException();


        public ValueTask<Salary> RetrieveSalaryByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
