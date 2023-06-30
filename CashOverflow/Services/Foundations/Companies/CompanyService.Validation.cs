// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Data;
using System.Reflection.Metadata;
using CashOverflow.Models.Companies;
using CashOverflow.Models.Companies.Exceptions;

namespace CashOverflow.Services.Foundations.Companies
{
    public partial class CompanyService
    {
        private void ValidateCompanyOnModify(Company company)
        {
            ValidateCompanyNotNull(company);
            
            Validate(
                (Rule:IsInvalid(company.Id), Parameter: nameof(Company.Id)),
                (Rule:IsInvalid(company.Name), Parameter: nameof(Company.Name)),
                (Rule:IsInvalid(company.CreatedDate), Parameter: nameof(Company.CreatedDate)));
        }

        private void ValidateAgainstStorageOnModify(Company inputCompany, Company storageCompany)
        {
            ValidateStorageCompanyExists(storageCompany, inputCompany.Id);

            Validate(
                (Rule: IsNotSame(
                    firstDate : inputCompany.CreatedDate,
                    secondDate: storageCompany.CreatedDate,
                    secondDateName: nameof(Company.CreatedDate)),
                    Parameter: nameof(Company.CreatedDate)));
        }
        
        private void ValidateStorageCompanyExists(Company maybeCompany, Guid companyId)
        {
            if (maybeCompany is null)
            {
                throw new NotFoundCompanyException(companyId);
            }
        }

        private void ValidateCompanyNotNull(Company company)
        {
            if (company is null)
            {
                throw new NullCompanyException();
            }
        }
        
        private static dynamic IsNotSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate != secondDate,
                Message = $"Date is not same as {secondDateName}"
            };
        
        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };
        
        private static dynamic IsInvalid(Guid companyId) => new
        {
            Condition = companyId == default,
            Message = "Id is required"
        };
        
        private static dynamic IsInvalid(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

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