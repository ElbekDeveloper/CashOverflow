// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using CashOverflow.Models.Salaries;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CashOverflow.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Salary> Salaries { get; set; }

        public async ValueTask<Salary> InsertSalaryAsync(Salary salary) =>
            await InsertAsync(salary);
    }
}
