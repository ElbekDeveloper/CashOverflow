// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using Xeptions;

namespace CashOverflow.Models.Locations.Exceptions
{
    public class AlreadyExistsLocationException : Xeption
    {
        public AlreadyExistsLocationException(Exception innerException)
            : base(message: "Location already exists.", innerException)
        { }
    }
}

