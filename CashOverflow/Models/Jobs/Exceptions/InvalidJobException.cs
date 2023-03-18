// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using Xeptions;

namespace CashOverflow.Models.Jobs.Exceptions
{
    public class InvalidJobException : Xeption
    {
        public InvalidJobException()
            : base(message: "Job is invalid.")
        { }
    }
}
