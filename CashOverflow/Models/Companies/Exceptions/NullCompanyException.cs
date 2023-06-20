// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using Xeptions;

namespace CashOverflow.Models.Companies.Exceptions
{
    public class NullCompanyException : Xeption
    {
        public NullCompanyException()
            : base(message: "Company is null")
        { }
    }
}
