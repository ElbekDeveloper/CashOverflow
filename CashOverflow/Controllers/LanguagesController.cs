// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
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
                when(languageValidationException.InnerException is InvalidLanguageException)
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
    }
}
