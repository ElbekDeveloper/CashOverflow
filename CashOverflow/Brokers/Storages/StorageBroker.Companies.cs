// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using CashOverflow.Models.Companies;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CashOverflow.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Company> Companies { get; set; }

        public async ValueTask<Company> UpdateCompanyAsync(Company company) =>
            await UpdateAsync(company);
    }
}