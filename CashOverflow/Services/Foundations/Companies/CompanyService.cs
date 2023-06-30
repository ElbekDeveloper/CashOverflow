// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using CashOverflow.Brokers.DateTimes;
using CashOverflow.Brokers.Loggings;
using CashOverflow.Brokers.Storages;
using CashOverflow.Models.Companies;
using System.Threading.Tasks;

namespace CashOverflow.Services.Foundations.Companies
{
    public partial class CompanyService : ICompanyService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public CompanyService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)

        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Company> ModifyCompanyAsync(Company company) =>
            TryCatch(async () =>
            {
                ValidateCompanyOnModify(company);
                
                Company maybeCompany =
                    await this.storageBroker.SelectCompanyByIdAsync(company.Id);

                ValidateAgainstStorageOnModify(inputCompany: company, storageCompany: maybeCompany);

                return await this.storageBroker.UpdateCompanyAsync(company);
            });
    }
}
