// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using CashOverflow.Models.Companies;
using System.Threading.Tasks;

namespace CashOverflow.Services.Foundations.Companies
{
    public interface ICompanyService
    {
        ValueTask<Company> ModifyCompanyAsync(Company company);
    }
}
