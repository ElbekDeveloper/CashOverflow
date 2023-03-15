using System;
using Xeptions;

namespace CashOverflow.Models.Locations.Exceptions
{
    public class FailedLocationServiceException : Xeption
    {
        public FailedLocationServiceException(Exception innerException)
            : base(message: "Failed location service error occurred, please contact support.", innerException)
        { } 
    }
}
