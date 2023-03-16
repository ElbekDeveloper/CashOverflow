// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using Xeptions;

namespace CashOverflow.Models.Salaries.Exceptions
{
    public class AlreadyExistsSalaryException : Xeption
    {
        public AlreadyExistsSalaryException(Exception innerException)
            : base(message: "Salary already exists.", innerException)
        { }
    }
}
