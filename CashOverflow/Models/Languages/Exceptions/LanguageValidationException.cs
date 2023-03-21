// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using Xeptions;

namespace CashOverflow.Models.Languages.Exceptions
{
    public class LanguageValidationException : Xeption
    {
        public LanguageValidationException(Exception innerException)
            : base(message: "Language validation error occured, fix the errors and try again", innerException)
        { }
    }
}
