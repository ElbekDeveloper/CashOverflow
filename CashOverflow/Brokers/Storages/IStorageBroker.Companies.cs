// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using CashOverflow.Models.Companies;

namespace CashOverflow.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Company> InsertCompanyAsync(Company company);
        IQueryable<Company> SelectAllCompanies();
        ValueTask<Company> SelectCompanyByIdAsync(Guid companyId);
        ValueTask<Company> DeleteCompanyAsync(Company company);
        ValueTask<Company> UpdateCompanyAsync(Company company);
    }
}
