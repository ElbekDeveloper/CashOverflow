// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using Xeptions;

namespace CashOverflow.Models.Reviews.Exceptions
{
    public class InvalidReviewReferenceException : Xeption
    {
        public InvalidReviewReferenceException(Exception innerException)
            : base(message: "Invalid review reference error occurred.", innerException)
        { }
    }
}
