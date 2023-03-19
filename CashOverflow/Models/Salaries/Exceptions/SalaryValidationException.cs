// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using Xeptions;

namespace CashOverflow.Models.Salaries.Exceptions
{
    public class SalaryValidationException : Xeption
    {
        public SalaryValidationException(Xeption innerException)
            : base(message: "Salary validation error occurred, fix the error, and try again!", innerException)
        { }
    }
}
