// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using Xeptions;

namespace CashOverflow.Models.Locations.Exceptions
{
    public class FailedLocationServiceException : Xeption
    {
        public FailedLocationServiceException(Exception innerException)
            : base(message: "Service error occured, contact support.", innerException)
        { }
    }
}
