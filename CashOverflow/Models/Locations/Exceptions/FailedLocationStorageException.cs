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
           : base(message: "Failed language storage error occurred, contact support.", innerException)
        { }
    }
}
