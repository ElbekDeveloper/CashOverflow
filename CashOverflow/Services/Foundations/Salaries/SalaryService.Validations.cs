// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Data;
using System.Reflection.Metadata;
using CashOverflow.Models.Salaries;
using CashOverflow.Models.Salaries.Exceptions;

namespace CashOverflow.Services.Foundations.Salaries
{
    public partial class SalaryService
    {
        private void ValidateSalaryOnAdd(Salary salary)
        {
            ValidateSalaryNotNull(salary);

            Validate(
                (Rule: IsInvalid(salary.Id), Parameter: nameof(Salary.Id)),
                (Rule: IsInvalid(salary.Amount), Parameter: nameof(Salary.Amount)),
                (Rule: IsInvalid(salary.Experience), Parameter: nameof(Salary.Experience)),
                (Rule: IsInvalid(salary.CreatedDate), Parameter: nameof(Salary.CreatedDate)),
                (Rule: IsNotRecent(date: salary.CreatedDate), Parameter: nameof(Salary.CreatedDate))); 
        }
        private static void ValidateSalaryNotNull(Salary salary)
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

        private dynamic IsNotRecent(DateTimeOffset date) => new
        {
            Condition = IsDateNotRecent(date),
            Message = "Date is not recent"
        };

        private bool IsDateNotRecent(DateTimeOffset date)//10:51:20
        {
            DateTimeOffset currentDate = this.dateTimeBroker.GetCurrentDateTimeOffset();//10:51:00
            TimeSpan timeDifference = currentDate.Subtract(date); //-20

            return timeDifference.TotalSeconds is > 60 or < 0;
        }

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
