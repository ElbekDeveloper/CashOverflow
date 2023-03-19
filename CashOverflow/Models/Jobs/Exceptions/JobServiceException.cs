﻿// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using Xeptions;

namespace CashOverflow.Models.Jobs.Exceptions
{
    public class JobServiceException : Xeption
    {
        public JobServiceException(Xeption innerException)
            : base(message: "Job service error occured, contact support.", innerException)
        { }
    }
}

