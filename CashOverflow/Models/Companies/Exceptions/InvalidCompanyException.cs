// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using Xeptions;

namespace CashOverflow.Models.Companies.Exceptions
{
    public class InvalidCompanyException : Xeption
    {
        public InvalidCompanyException()
            : base(message: "Company is invalid")
        { }
    }
}
