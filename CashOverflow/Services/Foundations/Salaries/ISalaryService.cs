// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using CashOverflow.Models.Salaries;

namespace CashOverflow.Services.Foundations.Salaries
{
    public interface ISalaryService
    {
        ValueTask<Salary> AddSalaryAsync(Salary salary);
        IQueryable<Salary> RetrieveAllSalary();
    }
}
