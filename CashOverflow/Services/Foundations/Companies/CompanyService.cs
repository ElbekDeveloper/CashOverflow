// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Threading.Tasks;
using CashOverflow.Models.Companies;

namespace CashOverflow.Services.Foundations.Companies
{
    public partial class CompanyService : ICompanyService
    {
        public ValueTask<Company> RemoveCompanyByIdAsync(Guid companyId) =>    
            throw new NotImplementedException();
    }
}
