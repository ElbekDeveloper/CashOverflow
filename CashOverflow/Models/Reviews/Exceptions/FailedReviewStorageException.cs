// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using Xeptions;

namespace CashOverflow.Models.Reviews.Exceptions
{
    public class FailedReviewStorageException : Xeption
    {
        public FailedReviewStorageException(Exception innerException)
            : base(message: "Failed review storage error occurred, contact support.", innerException)
        { }
    }
}

