// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using Xeptions;

namespace CashOverflow.Models.Languages.Exceptions
{
    public class AlreadyExistsLanguageException : Xeption
    {
        public AlreadyExistsLanguageException(Exception innerException)
            : base(message: "Already exists exception", innerException) 
        { }
    }
}
