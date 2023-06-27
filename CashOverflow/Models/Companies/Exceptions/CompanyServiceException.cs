// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using Xeptions;

namespace CashOverflow.Models.Companies.Exceptions
{
    public class CompanyServiceException : Xeption
    {
        public CompanyServiceException(Exception innerException) 
            : base(message: "Company service error occurred, please contact support", innerException)
        { }
    }
}