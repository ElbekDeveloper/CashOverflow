// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System.Threading.Tasks;
using CashOverflow.Models.Companies;

namespace CashOverflow.Services.Foundations.Companies
{
    public interface ICompanyService
    {
        /// <exception cref="Models.Companies.Exceptions.CompanyValidationException"></exception>
        /// <exception cref="Models.Companies.Exceptions.CompanyDependencyException"></exception>
        /// <exception cref="Models.Companies.Exceptions.CompanyDependencyValidationException"></exception>
        /// <exception cref="Models.Companies.Exceptions.CompanyServiceException"></exception>
        ValueTask<Company> AddCompanyAsync(Company company);

        /// <exception cref="Models.Companies.Exceptions.CompanyValidationException"></exception>
        /// <exception cref="Models.Companies.Exceptions.CompanyDependencyException"></exception>
        /// <exception cref="Models.Companies.Exceptions.CompanyServiceException"></exception>
        ValueTask<Company> ModifyCompanyAsync(Company company);
    }
}
