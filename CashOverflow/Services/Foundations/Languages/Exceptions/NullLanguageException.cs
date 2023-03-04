// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using Xeptions;

namespace CashOverflow.Services.Foundations.Languages.Exceptions
{
    public class NullLanguageException : Xeption
    {
        public NullLanguageException()
            : base(message: "Language is null")
        { }
    }
}
