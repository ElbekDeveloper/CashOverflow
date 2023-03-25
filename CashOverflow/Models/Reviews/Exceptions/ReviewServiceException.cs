// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using Xeptions;

namespace CashOverflow.Models.Reviews.Exceptions
{
    public class ReviewServiceException : Xeption
    {
        public ReviewServiceException(Exception innerException)
            : base(message: "Review service error occured, contact support", innerException)
        { }
    }
}
