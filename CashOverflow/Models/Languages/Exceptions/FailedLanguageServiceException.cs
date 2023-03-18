// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using Xeptions;

namespace CashOverflow.Models.Languages.Exceptions
{
    public class FailedLanguageServiceException : Xeption
    {

        public FailedLanguageServiceException(Exception innerException)
            : base(message: "Failed language service error occured, please contact support", innerException)
        { }
    }
}
