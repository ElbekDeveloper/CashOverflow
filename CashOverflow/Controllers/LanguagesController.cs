// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System.Linq;
using CashOverflow.Models.Languages;
using CashOverflow.Models.Locations.Exceptions;
using CashOverflow.Models.Locations;
using CashOverflow.Services.Foundations.Languages;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using CashOverflow.Models.Languages.Exceptions;

namespace CashOverflow.Controllers
{
    [ApiController]
    [Route("api/(controller)")]
    public class LanguagesController : RESTFulController
    {
        private readonly ILanguageService languageService;

        public LanguagesController(ILanguageService languageService) =>
            this.languageService = languageService;

        [HttpGet]
        public ActionResult<IQueryable<Language>> GetAllLanguages()
        {
            try
            {
                IQueryable<Language> allLanguages = this.languageService.RetrieveAllLanguages();

                return Ok(allLanguages);
            }
            catch (LanguageDependencyException locationDependencyException)
            {
                return InternalServerError(locationDependencyException.InnerException);
            }
            catch (LanguageServiceException languageServiceException)
            {
                return InternalServerError(languageServiceException.InnerException);
            }
        }
    }

}

