// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using Xeptions;

namespace CashOverflow.Models.Companies.Exceptions
{
    public class AlreadyExistsCompanyException : Xeption
    {
        public AlreadyExistsCompanyException(Exception innerException)
            : base(message: "Company already exists.", innerException)
        { }
    }
}
