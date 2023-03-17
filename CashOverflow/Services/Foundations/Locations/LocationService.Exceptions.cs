// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Threading.Tasks;
using CashOverflow.Models.Locations;
using CashOverflow.Models.Locations.Exceptions;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Xeptions;

namespace CashOverflow.Services.Foundations.Locations
{
    public partial class LocationService
    {
        private delegate ValueTask<Location> ReturningLocationFunction();

        private async ValueTask<Location> TryCatch(ReturningLocationFunction returningLocationFunction)
        {
            try
            {
                return await returningLocationFunction();
            }
            catch (InvalidLocationException invalidLocationException)
            {
                throw CreateAndLogValidationsException(invalidLocationException);
            }
            catch (NotFoundLocationException notFoundLocationException)
            {
                throw CreateAndLogValidationsException(notFoundLocationException);
            }
            catch (SqlException sqlException)
            {
                var failedLocationStoragException = new FailedLocationStorageException(sqlException);

                throw CreateAndLogDependencyException(failedLocationStoragException);
            }
            catch (Exception exception)
            {
                var failedLocationServiceException = new FailedLocationServiceException(exception);

                throw CreateAndLogServiceException(failedLocationServiceException);
            }
        }

        private LocationValidationException CreateAndLogValidationsException(Xeption exception)
        {
            var locationValidationException = new LocationValidationException(exception);
            this.loggingBroker.LogError(locationValidationException);

            return locationValidationException;
        }

        private LocationDependencyException CreateAndLogDependencyException(Xeption xeption)
        {
            var locationDependencyException = new LocationDependencyException(xeption);
            this.loggingBroker.LogCritical(locationDependencyException);

            return locationDependencyException;
        }

        private LocationServiceException CreateAndLogServiceException(Xeption innerException)
        {
            var locationServiceException = new LocationServiceException(innerException);
            this.loggingBroker.LogError(locationServiceException);

            return locationServiceException;
        }
    }
}
