// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using Xeptions;

namespace CashOverflow.Models.Reviews.Exceptions
{
    public class ReviewDependencyException : Xeption
    {
        public ReviewDependencyException(Exception innerException)
            : base(message: "Review dependency error occurred, contact support.", innerException)
        { }
    }
}
