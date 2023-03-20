// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using CashOverflow.Models.Locations;
using CashOverflow.Models.Locations.Exceptions;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace CashOverflow.Services.Foundations.Locations
{
    public partial class LocationService
    {
        private delegate ValueTask<Location> ReturningLocationFunction();
        private delegate IQueryable<Location> ReturningLocationsFunction();

        private async ValueTask<Location> TryCatch(ReturningLocationFunction returningLocationFunction)
        {
            try
            {
                return await returningLocationFunction();
            }
            catch (InvalidLocationException invalidLocationException)
            {
                throw CreateAndLogValidationException(invalidLocationException);
            }
            catch (NullLocationException nullLocationException)
            {
                throw CreateAndLogValidationException(nullLocationException);
            }
            catch (NotFoundLocationException notFoundLocationException)
            {
                throw CreateAndLogValidationException(notFoundLocationException);
            }
            catch (SqlException sqlException)
            {
                var failedLocationStorageException = new FailedLocationStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedLocationStorageException);
            }
            catch (DuplicateKeyException dublicateKeyException)
            {
                var alreadyExistsLocationException = new AlreadyExistsLocationException(dublicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsLocationException);
            }
            catch (DbUpdateConcurrencyException databaseUpdateConcurrencyException)
            {
                var lockedLocationException =
                    new LockedLocationException(databaseUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(lockedLocationException);
            }
            catch (Exception exception)
            {
                FailedLocationServiceException failedLocationServiceException = new FailedLocationServiceException(exception);

                throw CreateAndLogServiceException(failedLocationServiceException);
            }
        }

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
                var failedLocationServiceException = new FailedLocationServiceException(exception);

                throw CreateAndLogServiceException(failedLocationServiceException);
            }
        }

        private LocationValidationException CreateAndLogValidationException(Xeption exception)
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

        private LocationDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var locationDependencyException = new LocationDependencyException(exception);
            this.loggingBroker.LogCritical(locationDependencyException);

            return locationDependencyException;
        }

        private LocationDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var locationDependencyValidationException =
                new LocationDependencyValidationException(exception);

            this.loggingBroker.LogError(locationDependencyValidationException);

            return locationDependencyValidationException;
        }

        private LocationServiceException CreateAndLogServiceException(Xeption exception)
        {
            var locationServiceException = new LocationServiceException(exception);

            this.loggingBroker.LogError(locationServiceException);

            return locationServiceException;
        }
    }
}
