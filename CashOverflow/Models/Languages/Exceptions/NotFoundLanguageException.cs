// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using Xeptions;

namespace CashOverflow.Models.Languages.Exceptions {
    public class NotFoundLanguageException : Xeption {
        public NotFoundLanguageException(Guid languageId) :
            base(message: $"Couldn't found language with id: {languageId}.") {
        }
    }
}
