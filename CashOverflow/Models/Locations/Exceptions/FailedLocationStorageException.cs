// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using Xeptions;

namespace CashOverflow.Models.Locations.Exceptions
{
    public class FailedLocationStorageException : Xeption
    {
        public FailedLocationStorageException(Exception innerException)
            : base(message: "Failed location storage error occured, contact support.", 
                  innerException)
        { }
    }
}
