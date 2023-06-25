// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using CashOverflow.Models.Companies;
using CashOverflow.Models.Companies.Exceptions;

namespace CashOverflow.Services.Foundations.Companies
{
    public partial class CompanyService
    {
        private static void ValidateStorageCompanyExists(Company maybeCompany, Guid companyId)
        {
            if (maybeCompany is null)
            {
                throw new NotFoundCompanyException(companyId);
            }
        }

        private static void ValidateCompanyId(Guid companyId) =>
            Validate((Rule: IsInvalid(companyId), Parameter: nameof(Company.Id)));

        private static dynamic IsInvalid(Guid companyId) => new
        {
            Condition = companyId == Guid.Empty,
            Message = "Id is required"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidCompanyException = new InvalidCompanyException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidCompanyException.AddData(
                        key: parameter,
                        values: rule.Message);
                }
            }

            invalidCompanyException.ThrowIfContainsErrors();
        }
    }
}
