// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using Xeptions;

namespace CashOverflow.Models.Locations.Exceptions
{
    public class LockedLocationException : Xeption
    {
        public LockedLocationException(Exception innerException)
            : base(message: "Locked Location record error, contact support.", innerException)
        { }
    }
}
