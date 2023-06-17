using CashOverflow.Models.Companies;
using System.Threading.Tasks;

namespace CashOverflow.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Company> UpdateCompanyAsync(Company company);
    }
}
