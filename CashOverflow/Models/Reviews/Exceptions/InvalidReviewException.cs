// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using Xeptions;

namespace CashOverflow.Models.Reviews.Exceptions
{
    public class InvalidReviewException : Xeption
    {
        public InvalidReviewException()
              : base(message: "Review is invalid.")
        { }
    }
}

