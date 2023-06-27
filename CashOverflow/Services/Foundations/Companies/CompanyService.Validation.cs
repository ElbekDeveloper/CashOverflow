// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using CashOverflow.Models.Companies;
using CashOverflow.Models.Companies.Exceptions;

namespace CashOverflow.Services.Foundations.Companies
{
    public partial class CompanyService
    {
        private void ValidateCompanyOnModify(Company company) =>
            ValidateCompanyNotNull(company);

        private void ValidateCompanyNotNull(Company company)
        {
            if (company is null)
            {
                throw new NullCompanyException();
            }
        }
    }
}