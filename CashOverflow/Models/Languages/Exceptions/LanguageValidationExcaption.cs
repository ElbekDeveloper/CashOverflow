using Xeptions;

namespace CashOverflow.Models.Languages.Exceptions
{
    public class LanguageValidationExcaption : Xeption
    {
        public LanguageValidationExcaption(Xeption innerException)
            : base(message: "Language validation error occured, fix the error and try again", innerException)
        {

        }
    }
}
