// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System.Linq;
using CashOverflow.Models.Companies;
using Microsoft.EntityFrameworkCore;

namespace CashOverflow.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Company> Companies { get; set; }
        public IQueryable<Company> SelectAllCompanies() =>
            SelectAll<Company>();
    }
}