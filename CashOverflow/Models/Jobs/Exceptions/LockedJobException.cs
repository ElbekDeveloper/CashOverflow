// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using Xeptions;

namespace CashOverflow.Models.Jobs.Exceptions
{
    public class LockedJobException : Xeption
    {
        public LockedJobException(Exception innerException)
            : base(message: "Job is locked, please try again.", innerException)
        { }
    }
}
