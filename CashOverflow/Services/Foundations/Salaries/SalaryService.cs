using CashOverflow.Brokers.Storages;
using CashOverflow.Models.Salaries;
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
        
    }
}
