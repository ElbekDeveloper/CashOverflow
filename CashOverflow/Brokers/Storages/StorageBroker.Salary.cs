// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using CashOverflow.Models.Salaries;
using Microsoft.EntityFrameworkCore;

namespace CashOverflow.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Salary> Salaries { get; set; }

        public async ValueTask<Salary> InsertSalaryAsync(Salary salary) =>
            await InsertAsync(salary);

        public IQueryable<Salary> SelectAllSalaries() => SelectAll<Salary>();

        public async ValueTask<Salary> SelectSalaryByIdAsync(Guid salaryId) =>
             await SelectAsync<Salary>(salaryId);
    }
}
