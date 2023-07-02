// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using CashOverflow.Models.Companies;
using Microsoft.EntityFrameworkCore;

namespace CashOverflow.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Company> Companies { get; set; }

        public async ValueTask<Company> InsertCompanyAsync(Company company) =>
            await InsertAsync(company);

        public IQueryable<Company> SelectAllCompanies() =>
            SelectAll<Company>();

        public async ValueTask<Company> SelectCompanyByIdAsync(Guid companyId) =>
            await SelectAsync<Company>(companyId);

        public async ValueTask<Company> DeleteCompanyAsync(Company company) =>
            await DeleteAsync(company);

        public async ValueTask<Company> UpdateCompanyAsync(Company company) =>
            await UpdateAsync(company);
    }
}