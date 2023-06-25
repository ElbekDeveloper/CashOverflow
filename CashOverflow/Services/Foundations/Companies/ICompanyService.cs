// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Threading.Tasks;
using CashOverflow.Models.Companies;

namespace CashOverflow.Services.Foundations.Companies
{
    public partial interface ICompanyService
    {
        ValueTask<Company> RemoveCompanyByIdAsync(Guid id);
    }
}
