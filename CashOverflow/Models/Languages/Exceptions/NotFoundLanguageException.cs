// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using Xeptions;

namespace CashOverflow.Models.Languages.Exceptions
{
    public class NotFoundLanguageException : Xeption
    {
        private Guid someLanguageId;

        public NotFoundLanguageException(Guid someLanguageId)
            : base(message: $"Couldn't find Language with id: {someLanguageId}.")
        {

        }

    }
}
