using System;
using System.Threading.Tasks;
using CashOverflow.Models.Languages;

namespace CashOverflow.Services.Foundations.Languages
{
    public interface ILanguageService
    {
        ValueTask<Language> RemoveLanguageByIdAsync(Guid languageId);
    }
}