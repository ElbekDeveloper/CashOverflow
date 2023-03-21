// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System.Linq;
using CashOverflow.Models.Salaries;
using CashOverflow.Models.Salaries.Exceptions;
using CashOverflow.Services.Foundations.Salaries;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace CashOverflow.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalariesController :RESTFulController
    {
        private readonly ISalaryService salaryService;

        public SalariesController(ISalaryService salaryService) =>
            this.salaryService = salaryService;

        [HttpGet]
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