// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using Xeptions;

namespace CashOverflow.Models.Languages.Exceptions
{
    public class LockedLanguageException : Xeption
    {
        public LockedLanguageException(Exception innerException)
            : base(message: "Language is locked, please try again.", innerException)
        { }
    }
}
