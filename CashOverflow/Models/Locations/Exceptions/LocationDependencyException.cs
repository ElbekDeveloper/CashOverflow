// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using Xeptions;

namespace CashOverflow.Models.Locations.Exceptions
{
    public class LocationDependencyException : Xeption
    {
        public LocationDependencyException(Exception innerException)
            : base(message: "Location dependency error occured, contact support.", innerException)
        { }

    }
}
