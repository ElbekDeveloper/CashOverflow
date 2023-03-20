// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System.Threading.Tasks;
using System;
using CashOverflow.Services.Foundations.Jobs;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using CashOverflow.Models.Jobs;
using CashOverflow.Models.Jobs.Exceptions;

namespace CashOverflow.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobsController : RESTFulController
    {
        private readonly IJobService jobService;

        public JobsController(IJobService jobService) =>
            this.jobService = jobService;

        [HttpGet("{jobId}")]
        public async ValueTask<ActionResult<Job>> GetJobByIdAsync(Guid jobId)
        {
            try
            {
                return await this.jobService.RetrieveJobByIdAsync(jobId);
            }
            catch (JobDependencyException jobDependencyException)
            {
                return InternalServerError(jobDependencyException.InnerException);
            }
            catch (JobValidationException jobValidationException)
                when (jobValidationException.InnerException is InvalidJobException)
            {
                return BadRequest(jobValidationException.InnerException);
            }
            catch (JobValidationException jobValidationException)
                when (jobValidationException.InnerException is NotFoundJobException)
            {
                return NotFound(jobValidationException.InnerException);
            }
            catch (JobServiceException jobServiceException)
            {
                return InternalServerError(jobServiceException.InnerException);
            }
        }
    }
}
