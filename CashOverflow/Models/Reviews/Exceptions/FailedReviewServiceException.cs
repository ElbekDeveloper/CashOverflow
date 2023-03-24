// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using Xeptions;

namespace CashOverflow.Models.Reviews.Exceptions
{
    public class FailedReviewServiceException : Xeption
    {
        public FailedReviewServiceException(Exception innerException)
           : base(message: "Failed review service error occurred, contact support.", innerException)
        { }
    }
}
