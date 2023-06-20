// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using CashOverflow.Models.Companies;

namespace CashOverflow.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        IQueryable<Company> SelectAllCompanies();
        ValueTask<Company> InsertCompanyAsync(Company company);
        ValueTask<Company> DeleteCompanyAsync(Company company);
    }
}
