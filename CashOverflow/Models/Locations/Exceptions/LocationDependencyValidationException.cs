// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using Xeptions;

namespace CashOverflow.Models.Locations.Exceptions
{
    public class LocationDependencyValidationException : Xeption
    {
        public LocationDependencyValidationException(Xeption innerException)
            : base(message: "Location dependency validation error occured, contact support.",
                  innerException)
        { }
    }
}
