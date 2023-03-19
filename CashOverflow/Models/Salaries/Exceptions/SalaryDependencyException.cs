// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using Xeptions;

namespace CashOverflow.Models.Salaries.Exceptions
{
    public class SalaryDependencyException : Xeption
    {
        public SalaryDependencyException(Xeption innerException)
            : base(message: "Salary dependency exception occurred, contact support.", innerException)
        { }
    }
}
