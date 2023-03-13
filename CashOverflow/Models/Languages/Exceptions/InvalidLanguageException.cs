// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using Xeptions;

namespace CashOverflow.Models.Languages.Exceptions {
    public class InvalidLanguageException : Xeption {
        public InvalidLanguageException() :
            base(message: "Language is invalid.") {
        }
    }
}
