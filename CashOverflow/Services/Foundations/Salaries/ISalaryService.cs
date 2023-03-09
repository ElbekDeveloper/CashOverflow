<<<<<<< HEAD
﻿using System;
=======
﻿// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using CashOverflow.Models.Salaries;
using System;
>>>>>>> cd307bc66517cf430e3e32d96cdfcc10dbe2ca48
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
