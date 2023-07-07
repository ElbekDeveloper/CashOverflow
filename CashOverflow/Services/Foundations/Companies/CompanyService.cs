// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Threading.Tasks;
using CashOverflow.Brokers.DateTimes;
using CashOverflow.Brokers.Loggings;
using CashOverflow.Brokers.Storages;
using CashOverflow.Models.Companies;

namespace CashOverflow.Services.Foundations.Companies
{
    public partial class CompanyService : ICompanyService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public CompanyService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Company> AddCompanyAsync(Company company) =>
        TryCatch(async () =>
        {
            ValidateCompanyOnAdd(company);

            return await this.storageBroker.InsertCompanyAsync(company);
        });

        public ValueTask<Company> ModifyCompanyAsync(Company company) =>
        TryCatch(async () =>
        {
            ValidateCompanyOnModify(company);

            Company maybeCompany =
                await this.storageBroker.SelectCompanyByIdAsync(company.Id);

            ValidateAgainstStorageOnModify(inputCompany: company, storageCompany: maybeCompany);

            return await this.storageBroker.UpdateCompanyAsync(company);
        });

        public async ValueTask<Company> RemoveCompanyById(Guid companyId)
        {
            Company maybeCompany = await this.storageBroker.SelectCompanyByIdAsync(companyId);

            return await this.storageBroker.DeleteCompanyAsync(maybeCompany);
        }
    }
}
