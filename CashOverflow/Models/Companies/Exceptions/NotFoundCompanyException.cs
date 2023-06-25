// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using Xeptions;

namespace CashOverflow.Models.Companies.Exceptions
{
    public class NotFoundCompanyException : Xeption
    {
        public NotFoundCompanyException(Guid companyId) 
            : base(message: $"Couldn't find job with id: {companyId}.")
        { }
    }
}
