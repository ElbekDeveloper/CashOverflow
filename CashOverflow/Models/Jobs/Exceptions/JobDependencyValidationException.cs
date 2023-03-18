// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using Xeptions;

namespace CashOverflow.Models.Jobs.Exceptions
{
    public class JobDependencyValidationException : Xeption
    {
        public JobDependencyValidationException(Xeption innerException)
            : base(message: "Job dependency validation error occurred, fix the errors and try again.",
                  innerException)
        { }
    }
}

