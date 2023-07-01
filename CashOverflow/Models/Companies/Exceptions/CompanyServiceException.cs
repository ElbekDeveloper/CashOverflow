// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using Xeptions;

namespace CashOverflow.Models.Companies.Exceptions
{
    public class CompanyServiceException : Xeption
    {
        public CompanyServiceException(Xeption innerException)
            : base(message: "Company service error occurred, contact support.", innerException)
        { }
    }
}
