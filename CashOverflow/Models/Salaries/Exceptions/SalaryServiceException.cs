// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using Xeptions;

namespace CashOverflow.Models.Salaries.Exceptions
{
    public class SalaryServiceException : Xeption
    {
        public SalaryServiceException(Exception innerException)
            : base(message: "Salary service error occurred, contact support", innerException)
        { }
    }
}
