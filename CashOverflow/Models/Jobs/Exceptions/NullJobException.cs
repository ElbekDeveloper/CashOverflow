using Xeptions;

namespace CashOverflow.Models.Jobs.Exceptions
{
    public class NullJobException : Xeption
    {
        public NullJobException() : base(message: "Job is null")
        { }
    }
}
