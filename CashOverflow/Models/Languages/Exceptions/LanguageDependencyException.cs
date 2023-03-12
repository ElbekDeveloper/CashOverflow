// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using Xeptions;

namespace CashOverflow.Models.Languages.Exceptions
{
    public class LanguageDependencyException : Xeption
    {
        public LanguageDependencyException(Xeption innerException)
            : base(message: "Language dependency exception occurred, contact support", innerException)
        { }
    }
}
