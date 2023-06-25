// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Transactions;
using CashOverflow.Models.Companies;
using CashOverflow.Models.Companies.Exceptions;

namespace CashOverflow.Services.Foundations.Companies
{
    public partial class CompanyService
    {

        private void ValidateCompany(Company company)
        {
            ValidateCompanyNotNull(company);

            Validate(
                (Rule: IsInvalid(company.Id), Parameter: nameof(Company.Id)),
                (Rule: IsInvalid(company.Name), Parameter: nameof(Company.Name)),
                (Rule: IsInvalid(company.CreatedDate), Parameter: nameof(Company.CreatedDate)),
                (Rule: IsNotRecent(company.CreatedDate), Parameter: nameof(Company.CreatedDate)));
        }

        private static void ValidateCompanyNotNull(Company company)
        {
            if (company is null)
            {
                throw new NullCompanyException();
            }
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Value is required"
        };

        private dynamic IsNotRecent(DateTimeOffset date) => new
        {
            Condition = IsDateNotRecent(date),
            Message = "Value is not recent"
        };

        private bool IsDateNotRecent(DateTimeOffset date)
        {
            DateTimeOffset now = this.dateTimeBroker.GetCurrentDateTimeOffset();
            TimeSpan timeDifference = now.Subtract(date);

            return timeDifference.TotalSeconds is > 60 or < 0;
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidCompanyException = new InvalidCompanyException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidCompanyException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidCompanyException.ThrowIfContainsErrors();
        }
    }
}
