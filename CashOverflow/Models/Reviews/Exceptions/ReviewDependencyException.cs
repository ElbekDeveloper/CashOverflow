// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using Xeptions;

namespace CashOverflow.Models.Reviews.Exceptions
{
    public class ReviewDependencyException : Xeption
    {
        public ReviewDependencyException(Xeption innerException)
            : base(message: "Review dependency error occured, contact support", innerException)
        {}
    }
}
