// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using Xeptions;

namespace CashOverflow.Models.Salaries.Exceptions
{
    public class FailedSalaryServiceException : Xeption
    {
        public FailedSalaryServiceException(Exception innerException)
            : base(message: "Failed salary service error occurred, contact support.", innerException)
        { }
    }
}
