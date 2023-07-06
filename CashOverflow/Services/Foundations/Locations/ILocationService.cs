// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using CashOverflow.Models.Locations;

namespace CashOverflow.Services.Foundations.Locations
{
    public interface ILocationService
    {
        /// <exception cref="Models.Locations.Exceptions.LocationValidationException"></exception>
        /// <exception cref="Models.Locations.Exceptions.LocationDependencyValidationException"></exception>
        /// <exception cref="Models.Locations.Exceptions.LocationDependencyException"></exception>
        /// <exception cref="Models.Locations.Exceptions.LocationServiceException"></exception>
        ValueTask<Location> AddLocationAsync(Location location);

        /// <exception cref="Models.Locations.Exceptions.LocationDependencyException"></exception>
        /// <exception cref="Models.Locations.Exceptions.LocationServiceException"></exception>     
        IQueryable<Location> RetrieveAllLocations();

        /// <exception cref="Models.Locations.Exceptions.LocationDependencyException"></exception>
        /// <exception cref="Models.Locations.Exceptions.LocationServiceException"></exception>   
        ValueTask<Location> RetrieveLocationByIdAsync(Guid locationId);

        /// <exception cref="Models.Locations.Exceptions.LocationValidationException"></exception>
        /// <exception cref="Models.Locations.Exceptions.LocationDependencyValidationException"></exception>
        /// <exception cref="Models.Locations.Exceptions.LocationDependencyException"></exception>
        /// <exception cref="Models.Locations.Exceptions.LocationServiceException"></exception>
        ValueTask<Location> ModifyLocationAsync(Location location);
        ValueTask<Location> RemoveLocationByIdAsync(Guid locationId);
    }
}
