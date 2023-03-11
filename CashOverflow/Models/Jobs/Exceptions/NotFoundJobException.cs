// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using Xeptions;

namespace CashOverflow.Models.Jobs.Exceptions
{
    public class NotFoundJobException : Xeption
    {
        public NotFoundJobException(Guid jobId)
           : base(message: $"Couldn't find job with id:{jobId}.")
        { }
    }
}
