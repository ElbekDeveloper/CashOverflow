// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using Xeptions;

namespace CashOverflow.Models.Salaries.Exceptions
{
    public class InvalidSalaryException : Xeption
    {
        public InvalidSalaryException()
            : base(message: "Salary is invalid.")
        {}
    }
}
