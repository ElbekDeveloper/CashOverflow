﻿// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using Xeptions;

namespace CashOverflow.Models.Languages.Exceptions
{
    public class FailedLanguageStorageException : Xeption
    {
        public FailedLanguageStorageException(Exception innerException)
            : base(message: "Failed language storage exception occurred, contact support", innerException)
        { }
    }
}
