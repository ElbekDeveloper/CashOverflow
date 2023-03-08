// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using Xeptions;

namespace CashOverflow.Models.Jobs.Exceptions
{
    public class JobDependencyException: Xeption
    {
        public JobDependencyException(Xeption innerException)
            : base(message: "Job dependency error occured, contact support.", innerException)
        { }
    }
}