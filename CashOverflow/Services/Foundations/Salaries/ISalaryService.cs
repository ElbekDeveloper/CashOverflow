// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using CashOverflow.Models.Salaries;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CashOverflow.Services.Foundations.Salaries
{
    public interface ISalaryService
    {
        ValueTask<Salary> AddSalaryAsync(Salary salary);
        IQueryable<Salary> RetrieveSalaryAll();
        ValueTask<Salary> RetriveSalaryByIdAsync(Guid id);
    }
}
