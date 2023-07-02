// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System.Threading.Tasks;
using CashOverflow.Models.Companies;
using CashOverflow.Models.Companies.Exceptions;
using CashOverflow.Services.Foundations.Companies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using RESTFulSense.Controllers;

namespace CashOverflow.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompaniesController : RESTFulController
    {
        private readonly ICompanyService companyService;

        public CompaniesController(ICompanyService companyService) =>
            this.companyService = companyService;

        [HttpPost]
        public async ValueTask<ActionResult<Company>> PostCompanyAsync(Company company)
        {
            try
            {
                Company postedCompany = await this.companyService.AddCompanyAsync(company);

                return Created(postedCompany);
            }
            catch (CompanyValidationException companyValidationException)
            {
                return BadRequest(companyValidationException.InnerException);
            }
            catch(CompanyDependencyException companyDependencyException)
            {
                return InternalServerError(companyDependencyException.InnerException);
            }
            catch (CompanyDependencyValidationException companyDependencyValidationException)
                when(companyDependencyValidationException.InnerException is AlreadyExistsCompanyException)
            {
                return Conflict(companyDependencyValidationException.InnerException);
            }
            catch (CompanyDependencyValidationException companyDependencyValidationException)
            {
                return BadRequest(companyDependencyValidationException.InnerException);
            }
            catch(CompanyServiceException companyServiceException)
            {
                return InternalServerError(companyServiceException.InnerException);
            }
        }
    }
}
