using Xeptions;

namespace CashOverflow.Services.Foundations.Languages.Exceptions
{
    public class LanguageValidationException : Xeption
    {
        public LanguageValidationException(Xeption innerException)
            :base(message: "Language validation error occured, fix the error and try again", innerException)
        {
            
        }
    }
}
