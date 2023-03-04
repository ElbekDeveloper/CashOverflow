// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System.Threading.Tasks;
using CashOverflow.Models.Languages;

namespace CashOverflow.Services.Foundations.Languages
{
    public interface ILanguageService
    {
        ValueTask<Language> AddLanguageAsync(Language language);
    }
}
