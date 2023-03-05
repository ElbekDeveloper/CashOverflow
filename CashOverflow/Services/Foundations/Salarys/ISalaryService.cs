// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using CashOverflow.Models.Salaries;
using System.Threading.Tasks;

namespace CashOverflow.Services.Foundations.Salarys
{
    public interface ISalaryService
    {
        ValueTask<Salary> AddSalaryAsync(Salary salary);
    }
}
