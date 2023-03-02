// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using CashOverflow.Models.Languages;
using System.Threading.Tasks;

namespace CashOverflow.Services.Foundations.Languages
{
    public interface ILanguageService
    {
        ValueTask<Language> AddLanguageAsync(Language language);
    }
}
