// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using Xeptions;

namespace CashOverflow.Models.Companies.Exceptions
{
    public class CompanyValidationException : Xeption
    {
        public CompanyValidationException(Xeption innerException) 
            : base(message: "Company validation error occured, fix the errors and try again.", innerException) 
        { }
    }
}
