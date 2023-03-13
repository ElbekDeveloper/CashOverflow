// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using Xeptions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CashOverflow.Models.Jobs.Exceptions
{
    public class JobDependencyValidationException : Xeption
    {
        public JobDependencyValidationException(Xeption innerException)
            : base(message: "Job dependency validation error occurred, fix the errors and try again.", 
                  innerException)
        { }
    }
}
