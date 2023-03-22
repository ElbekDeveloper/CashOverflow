// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using Xeptions;

namespace CashOverflow.Models.Jobs.Exceptions
{
    public class AlreadyExistsJobException : Xeption
    {
        public AlreadyExistsJobException(Exception innerException)
            : base(message: "Job already exists.", innerException)
        { }
    }
}
