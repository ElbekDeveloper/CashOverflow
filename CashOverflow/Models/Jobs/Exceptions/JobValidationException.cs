using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Xeptions;

namespace CashOverflow.Models.Jobs.Exceptions
{
    public class JobValidationException : Xeption
    {
        public JobValidationException(Xeption innerException) 
            : base(message: "Job validation error occured, fix the errors and try again.", innerException)
        { }
    }
}
