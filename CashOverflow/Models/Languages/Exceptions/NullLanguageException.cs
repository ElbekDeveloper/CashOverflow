using Xeptions;

namespace CashOverflow.Models.Languages.Exceptions
{
    public class NullLanguageException : Xeption
    {
        public NullLanguageException()
            :base(message: "Language is null.")
        {}
    }
}
