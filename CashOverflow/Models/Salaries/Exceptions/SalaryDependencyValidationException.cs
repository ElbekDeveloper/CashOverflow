// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using Xeptions;

namespace CashOverflow.Models.Salaries.Exceptions
{
    public class SalaryDependencyValidationException : Xeption
    {
        public SalaryDependencyValidationException(Xeption innerException)
            : base(message: "Salary dependency validation error occured, fix the errors and try again.", innerException)
        {}
    }
}
