using Xeptions;

namespace CashOverflow.Services.Foundations.Salaries.Exeptions
{
    public class NullSalaryException : Xeption
    {

        public NullSalaryException()
            : base(message: "Salary is null.")
        {  }
    }
}
