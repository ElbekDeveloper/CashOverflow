// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using Xeptions;

namespace CashOverflow.Models.Jobs.Exceptions
{
    public class JobValidationException : Xeption
    {
        public JobValidationException(Xeption innerException) 
            : base(message: "Job validation error occured, fix the errors and try again.", innerException)
        { }
    }
}
