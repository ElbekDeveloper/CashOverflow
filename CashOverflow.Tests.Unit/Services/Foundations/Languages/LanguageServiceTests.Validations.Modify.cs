using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashOverflow.Models.Languages;
using CashOverflow.Models.Languages.Exceptions;
using Xunit;

namespace CashOverflow.Tests.Unit.Services.Foundations.Languages
{
    public partial class LanguageServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfLanguageIsNullAndLogItAsync()
        {
            //given
            Language nullLanguage= null;
            var nullLanguageException = new NullLanguageException();

            var excpectedLanguageValidationException =
                new LanguageValidationException(nullLanguageException);

            //when

        }
    }
}
