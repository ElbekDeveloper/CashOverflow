// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using CashOverflow.Models.Salaries;
using CashOverflow.Models.Salaries.Exceptions;
using CashOverflow.Services.Foundations.Salaries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using RESTFulSense.Controllers;

namespace CashOverflow.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalariesController : RESTFulController
    {
        private readonly ISalaryService salaryService;

        public SalariesController(ISalaryService salaryService) =>
            this.salaryService = salaryService;

        [HttpPost]
        public async ValueTask<ActionResult<Salary>> PostSalaryAsync(Salary salary)
        {
            try
            {
                Salary addedSalary = await this.salaryService.AddSalaryAsync(salary);

                return Created(addedSalary);
            }
            catch (SalaryValidationException salaryValidationException)
            {
                return BadRequest(salaryValidationException.InnerException);
            }
            catch (SalaryDependencyValidationException salaryDependencyValidationException)
                when (salaryDependencyValidationException.InnerException is AlreadyExistsSalaryException)
            {
                return Conflict(salaryDependencyValidationException.InnerException);
            }
            catch (SalaryDependencyException salaryDependencyException)
            {
                return InternalServerError(salaryDependencyException.InnerException);
            }
            catch (SalaryServiceException salaryServiceException)
            {
                return InternalServerError(salaryServiceException.InnerException);
            }
        }


        [HttpGet]
        [EnableQuery]
        public ActionResult<IQueryable<Salary>> GetAllSalaries()
        {
            try
            {
                IQueryable<Salary> allSalaries = this.salaryService.RetrieveAllSalaries();

                return Ok(allSalaries);
            }
            catch (SalaryDependencyException salaryDependencyException)
            {
                return InternalServerError(salaryDependencyException.InnerException);
            }
            catch (SalaryServiceException salaryServiceException)
            {
                return InternalServerError(salaryServiceException.InnerException);
            }
        }
    }
}
