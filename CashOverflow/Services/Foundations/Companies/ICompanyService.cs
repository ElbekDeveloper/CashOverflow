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
        ValueTask<Company> AddCompanyAsync(Company company);
        ValueTask<Company> ModifyCompanyAsync(Company company);
    }
}
