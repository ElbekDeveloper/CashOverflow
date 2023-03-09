using System;
using System.Linq;
using System.Threading.Tasks;
using CashOverflow.Models.Salaries;

namespace CashOverflow.Services.Foundations.Salaries
{
    public interface ISalaryService
    {
        ValueTask<Salary> AddSalaryAsync(Salary salary);
        IQueryable<Salary> RetrieveSalaryAll();
        ValueTask<Salary> RetriveSalaryByIdAsync(Guid id);
    }
}
