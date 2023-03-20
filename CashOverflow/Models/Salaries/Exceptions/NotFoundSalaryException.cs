using System;
using Xeptions;

namespace CashOverflow.Models.Salaries.Exceptions
{
    public class NotFoundSalaryException : Xeption
    {
        public NotFoundSalaryException(Guid salaryId)
            : base(message: $"Couldn't find salary with id: {salaryId}.")
        { }
    }
}
