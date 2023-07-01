// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using Xeptions;

namespace CashOverflow.Models.Companies.Exceptions
{
    public class CompanyDependencyValidationException : Xeption
    {
        public CompanyDependencyValidationException(Xeption innerException)
            : base(message: "Company dependency validation error occurred, fix the errors and try again.",
                  innerException)
        { }
    }
}
