// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using CashOverflow.Models.Languages;
using CashOverflow.Models.Languages.Exceptions;
using CashOverflow.Services.Foundations.Languages;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace CashOverflow.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LanguagesController : RESTFulController
    {
        private readonly ILanguageService languageService;

        public LanguagesController(ILanguageService languageService) =>
            this.languageService = languageService;

        [HttpGet("{languageId}")]
        public async ValueTask<ActionResult<Language>> GetLanguageByIdAsync(Guid languageId)
        {
            try
            {
                return await this.languageService.RetrieveLanguageByIdAsync(languageId);
            }
            catch (LanguageDependencyException languageDependencyException)
            {
                return InternalServerError(languageDependencyException.InnerException);
            }
            catch (LanguageValidationException languageValidationException)
                when (languageValidationException.InnerException is InvalidLanguageException)
            {
                return BadRequest(languageValidationException.InnerException);
            }
            catch (LanguageValidationException languageValidationException)
                when (languageValidationException.InnerException is NotFoundLanguageException)
            {
                return NotFound(languageValidationException.InnerException);
            }
            catch (LanguageServiceException languageServiceException)
            {
                return InternalServerError(languageServiceException.InnerException);
            }
        }

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

        [HttpDelete("{languageId}")]
        public async ValueTask<ActionResult<Language>> DeleteLanguageByIdAsync(Guid languageId)
        {
            try
            {
                Language deletedLanguage =
                    await this.languageService.RemoveLanguageByIdAsync(languageId);

                return Ok(deletedLanguage);
            }
            catch (LanguageValidationException languageValidationException)
                when (languageValidationException.InnerException is NotFoundLanguageException)
            {
                return NotFound(languageValidationException.InnerException);
            }
            catch (LanguageValidationException languageValidationException)
            {
                return BadRequest(languageValidationException.InnerException);
            }
            catch (LanguageDependencyValidationException languageDependencyValidationException)
                when (languageDependencyValidationException.InnerException is LockedLanguageException)
            {
                return Locked(languageDependencyValidationException.InnerException);
            }
            catch (LanguageDependencyValidationException languageDependencyValidationException)
            {
                return BadRequest(languageDependencyValidationException.InnerException);
            }
            catch (LanguageDependencyException languageDependencyException)
            {
                return InternalServerError(languageDependencyException.InnerException);
            }
            catch (LanguageServiceException languageServiceException)
            {
                return InternalServerError(languageServiceException.InnerException);
            }
        }
    }
}
