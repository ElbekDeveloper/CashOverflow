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
        public LocationServiceException(Exception innerException)
            :base(message: "Language service error occured, contact support", innerException) 
        { }
    }
}
