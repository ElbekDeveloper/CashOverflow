// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using Xeptions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CashOverflow.Models.Locations.Exceptions
{
    public class LocationServiceException : Xeption
    {
        public LocationServiceException(Xeption innerException)
            : base(message: "Location service error occurred, contact support.", innerException)
        { }
    }
}
