// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using Xeptions;

namespace CashOverflow.Models.Locations.Exceptions
{
    public class LocationDependencyException : Xeption
    {
        public LocationDependencyException(Xeption innerException)
            : base("Location dependency exception occured, contact support", innerException)
        { }
    }
}

