// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using CashOverflow.Models.Salaries;
using CashOverflow.Models.Salaries.Exceptions;

namespace CashOverflow.Services.Foundations.Salaries
{
    public partial class SalaryService
    {
        private static void ValidateSalaryNotNull(Salary salary)
        {
            if (salary is null)
            {
                throw new NullSalaryException();
            }
        }
    }
}
