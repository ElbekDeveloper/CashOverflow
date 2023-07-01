// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using Xeptions;

namespace CashOverflow.Models.Companies.Exceptions
{
    public class FailedCompanyServiceException : Xeption
    {
        public FailedCompanyServiceException(Exception innerException)
            : base(message: "Failed company service error occurred, contact support.", 
                  innerException)
        { }
    }
}
