// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using CashOverflow.Models.Salaries;
using CashOverflow.Models.Salaries.Exceptions;

namespace CashOverflow.Services.Foundations.Salaries
{
    public partial class SalaryService
    {
        private static void ValidateSalaryOnAdd(Models.Salaries.Salary salary)
        {
            ValidateSalaryNotNull(salary);

            Validate(
                (Rule: IsInvalid(salary.Id), Parameter: nameof(Salary.Id)),
                (Rule: IsInvalid(salary.Amount), Parameter: nameof(Salary.Amount)),
                (Rule: IsInvalid(salary.Experience), Parameter: nameof(Salary.Experience)),
                (Rule: IsInvalid(salary.CreatedDate), Parameter: nameof(Salary.CreatedDate)));
        }
        private static void ValidateSalaryNotNull(Models.Salaries.Salary salary)
        {
            if (salary is null)
            {
                throw new NullSalaryException();
            }
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(decimal amount) => new
        {
            Condition = amount == 0,
            Message = "Amount is required"
        };

        private static dynamic IsInvalid(int experience) => new
        {
            Condition = experience == 0,
            Message = "Experience is required"
        };

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidSalaryException = new InvalidSalaryException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidSalaryException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidSalaryException.ThrowIfContainsErrors();
        }
    }
}
