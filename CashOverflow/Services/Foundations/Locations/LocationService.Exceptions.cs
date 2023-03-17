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
using Microsoft.EntityFrameworkCore;
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
            catch (NullLocationException nullLocationException)
            {
                throw CreateAndLogValidationException(nullLocationException);
            }
            catch (InvalidLocationException invalidLocationException)
            {
                throw CreateAndLogValidationException(invalidLocationException);
            }
            catch (SqlException sqlException)
            {
                var failedLocationStorageException = new FailedLocationStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedLocationStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsLocationException = new AlreadyExistsLocationException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsLocationException);
            }
            catch (Exception exception)
            {
                var failedLocationServiceException = new FailedLocationServiceException(exception);

                throw CreateAndLogServiceException(failedLocationServiceException);
            }
            catch (DbUpdateConcurrencyException databaseUpdateConcurrencyException)
            {
                var lockedLocationException =
                    new LockedLocationException(databaseUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(lockedLocationException);
            }
            catch (NotFoundLocationException notFoundLocationException)
            {
                throw CreateAndLogValidationException(notFoundLocationException);
            }
        }

        private LocationDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var locationDependencyValidationException =
                new LocationDependencyValidationException(exception);

            this.loggingBroker.LogError(locationDependencyValidationException);

            return locationDependencyValidationException;
        }

        private LocationValidationException CreateAndLogValidationException(Xeption exception)
        {
            var locationValidationException = new LocationValidationException(exception);
            this.loggingBroker.LogError(locationValidationException);

            return locationValidationException;
        }

        private LocationDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var locationDependencyException = new LocationDependencyException(exception);
            this.loggingBroker.LogCritical(locationDependencyException);

            return locationDependencyException;
        }

        private LocationDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var locationDependencyValidationException = new LocationDependencyValidationException(exception);
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
