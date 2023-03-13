// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using CashOverflow.Models.Salaries;

namespace CashOverflow.Brokers.Storages {
    public partial interface IStorageBroker {
        ValueTask<Salary> InsertSalaryAsync(Salary salary);
        IQueryable<Salary> SelectAllSalaries();
        ValueTask<Salary> SelectSalaryByIdAsync(Guid salaryId);
    }
}