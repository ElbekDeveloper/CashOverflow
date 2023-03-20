// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using Xeptions;

namespace CashOverflow.Models.Salaries.Exceptions
{
    public class FailedSalaryStorageException : Xeption
    {
        public FailedSalaryStorageException(Exception innerException)
            : base(message: "Failed salary storage error occurred, contact support.", innerException)
        { }
    }
}
