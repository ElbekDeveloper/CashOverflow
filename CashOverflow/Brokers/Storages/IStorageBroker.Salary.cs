// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using CashOverflow.Models.Salaries;
using System.Threading.Tasks;

namespace CashOverflow.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Salary> InsertSalaryAsync(Salary salary);
        ValueTask<Salary> SelectSalaryByIdAsync(Guid salaryId);
    }
}