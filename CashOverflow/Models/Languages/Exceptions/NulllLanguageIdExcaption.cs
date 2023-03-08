using Xeptions;

namespace CashOverflow.Models.Languages.Exceptions
{
    public class NulllLanguageIdExcaption : Xeption
    {
        public NulllLanguageIdExcaption()
            : base(message: "Input Id is Null")
        {

        }
    }
}
