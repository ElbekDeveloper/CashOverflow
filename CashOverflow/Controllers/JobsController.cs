// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using CashOverflow.Models.Jobs;
using CashOverflow.Models.Jobs.Exceptions;
using CashOverflow.Services.Foundations.Jobs;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace CashOverflow.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobsController : RESTFulController
    {
        private readonly IJobService jobService;

        public JobsController(IJobService jobService) =>
            this.jobService = jobService;

        [HttpGet]
        public ActionResult<IQueryable<Job>> GetAllJobs()
        {
            try
            {
                IQueryable<Job> allJobs = this.jobService.RetrieveAllJobs();

                return Ok(allJobs);
            }
            catch (JobDependencyException jobDependencyException)
            {
                return InternalServerError(jobDependencyException);
            }
            catch (JobServiceException jobServiceException)
            {
                return InternalServerError(jobServiceException);
            }
        }

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

        [HttpPut]
        public async ValueTask<ActionResult<Job>> PutJobAsync(Job job)
        {
            try
            {
                Job modifiedJob = await this.jobService.ModifyJobAsync(job);

                return Ok(modifiedJob);
            }
            catch (JobValidationException jobValidationException)
                when (jobValidationException.InnerException is NotFoundJobException)
            {
                return NotFound(jobValidationException.InnerException);
            }
            catch (JobValidationException jobValidationException)
            {
                return BadRequest(jobValidationException.InnerException);
            }
            catch (JobDependencyValidationException jobDependencyValidationException)
                when (jobDependencyValidationException.InnerException is AlreadyExistsJobException)
            {
                return Conflict(jobDependencyValidationException.InnerException);
            }
            catch (JobDependencyException jobDependencyException)
            {
                return InternalServerError(jobDependencyException.InnerException);
            }
            catch (JobServiceException jobServiceException)
            {
                return InternalServerError(jobServiceException.InnerException);
            }
        }

        [HttpDelete("{jobId}")]
        public async ValueTask<ActionResult<Job>> DeleteJobByIdAsync(Guid jobId)
        {
            try
            {
                Job deletedJob =
                    await this.jobService.RemoveJobByIdAsync(jobId);

                return Ok(deletedJob);
            }
            catch (JobValidationException jobValidationException)
                when (jobValidationException.InnerException is NotFoundJobException)
            {
                return NotFound(jobValidationException.InnerException);
            }
            catch (JobValidationException jobValidationException)
            {
                return BadRequest(jobValidationException.InnerException);
            }
            catch (JobDependencyValidationException jobDependencyValidationException)
                when (jobDependencyValidationException.InnerException is LockedJobException)
            {
                return Locked(jobDependencyValidationException.InnerException);
            }
            catch (JobDependencyValidationException jobDependencyValidationException)
            {
                return BadRequest(jobDependencyValidationException.InnerException);
            }
            catch (JobDependencyException jobDependencyException)
            {
                return InternalServerError(jobDependencyException.InnerException);
            }
            catch (JobServiceException jobServiceException)
            {
                return InternalServerError(jobServiceException.InnerException);
            }
        }
    }
}
