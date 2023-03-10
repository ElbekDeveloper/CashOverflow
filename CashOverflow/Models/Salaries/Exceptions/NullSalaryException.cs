// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using Xeptions;

namespace CashOverflow.Models.Salaries.Exceptions
{
    public class NullSalaryException : Xeption
    {

        public NullSalaryException()
            : base(message: "Salary is null.")
        { }
    }
}
