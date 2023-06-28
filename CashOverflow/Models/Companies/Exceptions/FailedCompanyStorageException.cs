// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using Xeptions;

namespace CashOverflow.Models.Companies.Exceptions;

public class FailedCompanyStorageException : Xeption
{
    public FailedCompanyStorageException(Exception innerException)
        : base("Failed company storage error occurred, contact support.", innerException)
    { }
}