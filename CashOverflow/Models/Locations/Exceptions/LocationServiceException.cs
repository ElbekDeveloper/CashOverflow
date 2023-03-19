// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using Xeptions;

namespace CashOverflow.Models.Locations.Exceptions
{
    public class LocationServiceException : Xeption
    {
        public LocationServiceException(Xeption innerException)
            : base(message: "Location service error occurred, contact support.", innerException)
        { }
    }
}
