// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using Xeptions;

namespace CashOverflow.Models.Companies.Exceptions
{
    public class LockedCompanyException : Xeption
    {
        public LockedCompanyException(Exception innerException) 
            : base(message: "Company is locked, please try again", innerException)
        { }
    }
}