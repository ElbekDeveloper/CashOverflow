// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using Xeptions;

namespace CashOverflow.Models.Languages.Exceptions
{
    public class LanguageServiceException : Xeption
    {
        public LanguageServiceException(Xeption innerException)
            : base(message: "Language service error occurred, contact support.", innerException)
        { }
    }
}
