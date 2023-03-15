// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using Xeptions;

namespace CashOverflow.Models.Languages.Exceptions
{
    public class LanguageDependencyValidationException : Xeption
    {
        public LanguageDependencyValidationException(Xeption innerException)
            : base(message: "Language dependency validation error occurred, fix the errors and try again.",
                  innerException)
        { }
    }
}
