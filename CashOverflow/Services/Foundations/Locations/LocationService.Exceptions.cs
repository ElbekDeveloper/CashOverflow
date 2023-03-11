// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System.Linq;
using CashOverflow.Models.Locations;
using CashOverflow.Models.Locations.Exceptions;
using Microsoft.Data.SqlClient;
using Xeptions;

namespace CashOverflow.Services.Foundations.Locations
{
    public partial class LocationService
    {
        private delegate IQueryable<Location> ReturningLocationsFunction();

        private IQueryable<Location> TryCatch(ReturningLocationsFunction returningLocationsFunction)
        {
            try
            {
                return returningLocationsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedLocationServiceException =
                    new FailedLocationStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedLocationServiceException);
            }
        }
        
        private LocationDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var locationDependencyException =
                new LocationDependencyException(exception);

            this.loggingBroker.LogCritical(locationDependencyException);

            return locationDependencyException;
        }
    }
}
