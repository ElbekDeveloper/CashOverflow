// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using Xeptions;

namespace CashOverflow.Models.Jobs.Exceptions
{
    public class FailedJobStorageException: Xeption
    {
        public FailedJobStorageException(Exception innerException)
            :base(message: "Failed job storage error occurred, contact support.", innerException)
        { }
    }
}