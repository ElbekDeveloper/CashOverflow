// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using CashOverflow.Models.Companies;
using System.Threading.Tasks;

namespace CashOverflow.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Company> InsertCompanyAsync(Company company);
        ValueTask<Company> DeleteCompanyAsync(Company company);
        ValueTask<Company> UpdateCompanyAsync(Company company);
    }
}
