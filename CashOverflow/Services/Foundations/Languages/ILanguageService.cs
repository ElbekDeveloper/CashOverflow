// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System.Linq;
using CashOverflow.Models.Languages;

namespace CashOverflow.Services.Foundations.Languages
{
    public interface ILanguageService
    {
        IQueryable<Language> RetrieveAllLanguages();
<<<<<<< HEAD
=======
        ValueTask<Language> RetrieveLanguageByIdAsync(Guid languageId);
        ValueTask<Language> RemoveLanguageByIdAsync(Guid languageId);
>>>>>>> 616ecb1c0aef2aa1d1b94db757dc1fde39cf606f
    }
}
