using Xeptions;

namespace CashOverflow.Services.Foundations.Salaries.Exeptions
{
    public class SalaryValidationException : Xeption
    {
        public SalaryValidationException(Xeption innerException)
            : base(message: "Salary validation error occurred, fix the error, and try again!", innerException)
        { }
    }
}
