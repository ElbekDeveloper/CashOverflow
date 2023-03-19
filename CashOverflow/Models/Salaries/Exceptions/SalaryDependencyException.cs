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
        public SalaryDependencyException(Exception innerException)
            : base(message: "Salary dependency error occurred, contact support.", innerException)
        { }
    }
}
