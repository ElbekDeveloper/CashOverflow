// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System.Threading.Tasks;
using CashOverflow.Models.Locations;
using CashOverflow.Models.Locations.Exceptions;
using CashOverflow.Services.Foundations.Locations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using RESTFulSense.Controllers;

namespace CashOverflow.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationsController : RESTFulController
    {
        private readonly ILocationService locationService;

        public LocationsController(ILocationService locationService) =>
            this.locationService = locationService;

        [HttpPost]
        public async ValueTask<ActionResult<Location>> PostLocationAsync(Location location)
        {
            try
            {
                Location addedLocation = await this.locationService.AddLocationAsync(location);

                return Created(addedLocation);
            }
            catch(LocationValidationException locationValidationException)
            {
                return BadRequest(locationValidationException.InnerException);
            }
            catch(LocationDependencyValidationException locationDependencyValidationException)
                when (locationDependencyValidationException.InnerException is AlreadyExistsLocationException)
            {
                return Conflict(locationDependencyValidationException.InnerException);
            }
            catch(LocationDependencyException locationDependencyException)
            {
                return InternalServerError(locationDependencyException.InnerException);
            }
            catch(LocationServiceException locationServiceException)
            {
                return InternalServerError(locationServiceException.InnerException);
            }
        }
    }
}
