// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using CashOverflow.Models.Salaries;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace CashOverflow.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Salary> Salaries { get; set; }

        public async ValueTask<Salary> InsertSalaryAsync(Salary salary) =>
            await InsertAsync(salary);

        public async ValueTask<Salary> SelectSalaryByIdAsync(Guid salaryId) =>
            await SelectAsync<Salary>(salaryId);
    }
}
