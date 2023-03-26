// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using Xeptions;

namespace CashOverflow.Models.Reviews.Exceptions
{
    public class ReviewDependencyValidationException : Xeption
    {
        public ReviewDependencyValidationException(Xeption innerException)
            : base(message: "Review dependency validation error occured, fix the errors and try again.", innerException)
        { }
    }
}

