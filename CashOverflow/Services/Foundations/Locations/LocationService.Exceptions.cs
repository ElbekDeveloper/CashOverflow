// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Linq;
using CashOverflow.Models.Languages.Exceptions;
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
            catch (Exception exception)
            {
                var failedLocationServiceException =
                    new FailedLocationServiceException(exception);

                throw CreateAndLogServiceException(failedLocationServiceException);
            }
        }
        
        private LocationDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var locationDependencyException =
                new LocationDependencyException(exception);

            this.loggingBroker.LogCritical(locationDependencyException);

            return locationDependencyException;
        }

        private LocationServiceException CreateAndLogServiceException(Xeption exception)
        {
            var locationServiceException = new LocationServiceException(exception);
            this.loggingBroker.LogError(locationServiceException);

            return locationServiceException;
        }
    }
}
