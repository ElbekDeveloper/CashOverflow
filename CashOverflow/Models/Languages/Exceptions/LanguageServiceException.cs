// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using Xeptions;

namespace CashOverflow.Models.Languages.Exceptions
{
    public class LanguageServiceException : Xeption
    {
        public LanguageServiceException(Exception innerException)
            : base(message: "Language service error occured, contact support", innerException)
        { }
    }
}
