// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System.Threading.Tasks;
using CashOverflow.Brokers.Storages;
using CashOverflow.Models.Companies;

namespace CashOverflow.Services.Foundations.Companies
{
    public class CompanyService : ICompanyService
    {
        private readonly IStorageBroker storageBroker;

        public CompanyService(IStorageBroker storageBroker) =>
            this.storageBroker = storageBroker;

        public async ValueTask<Company> AddCompanyAsync(Company company) =>
             await this.storageBroker.InsertCompanyAsync(company);
    }
}
