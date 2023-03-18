// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using Xeptions;

namespace CashOverflow.Models.Salaries.Exceptions
{
    public class SalaryServiceException : Xeption
    {
        public SalaryServiceException(Xeption innerException)
            : base(message: "Salary service error occurred, contact support.", innerException)
        { }
    }
}
