// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using Xeptions;

namespace CashOverflow.Models.Reviews.Exceptions
{
    public class AlreadyExistsReviewException : Xeption
    {
        public AlreadyExistsReviewException(Exception innerException)
            : base(message: "Review already exists.", innerException)
        { }
    }
}

