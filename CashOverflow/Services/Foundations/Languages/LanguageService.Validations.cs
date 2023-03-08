using CashOverflow.Models.Languages.Exceptions;
using System;

namespace CashOverflow.Services.Foundations.Languages
{
    public partial class LanguageService
    {
        private static void ValidateLanguageNotNull(Guid languageId)
        {
            if (languageId == Guid.Empty)
            {
                throw new NulllLanguageIdExcaption();
            }
        }
    }
}
