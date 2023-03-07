using CashOverflow.Models.Salaries;
using System.Threading.Tasks;

namespace CashOverflow.Services.Foundations.Salaries
{
    public interface ISalaryService
    {
        ValueTask<Salary> AddSalaryAsync(Salary salary);
    }
}
