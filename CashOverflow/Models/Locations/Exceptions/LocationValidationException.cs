// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using Xeptions;

namespace CashOverflow.Models.Locations.Exceptions
{
    public class LocationValidationException : Xeption
    {
        public LocationValidationException(Exception innerXeption)
            : base(message: "Location validation error occured, fix the errors and try again.", innerXeption)
        { }
    }
}
