// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using Xeptions;

namespace CashOverflow.Models.Languages
{
    public class NullLanguageException : Xeption
    {
        public NullLanguageException()
            : base(message: "Null language")
        { }
    }
}
