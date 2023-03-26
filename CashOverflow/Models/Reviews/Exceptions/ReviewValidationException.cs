// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using Xeptions;

namespace CashOverflow.Models.Reviews.Exceptions
{
    public class ReviewValidationException : Xeption
    {
        public ReviewValidationException(Xeption innerException)
             : base(message: "Review validation error occurred, fix the errors and try again.", innerException)
        { }
    }
}

