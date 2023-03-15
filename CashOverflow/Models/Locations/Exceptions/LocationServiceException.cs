using System;
using Xeptions;

namespace CashOverflow.Models.Locations.Exceptions
{
    public class LocationServiceException : Xeption
    {
        public LocationServiceException(Exception innerException)
            : base(message: "Location service error occured, contact support.", innerException)
        { }
    }
}
