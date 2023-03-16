// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System.Threading.Tasks;
using CashOverflow.Models.Locations;
using CashOverflow.Models.Locations.Exceptions;
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
            catch (InvalidLocationException invalidLocationException)
            {
                throw CreateAndLogValidationException(invalidLocationException);
            }
            catch (NotFoundLocationException notFoundLocationException)
            {
                throw CreateAndLogValidationException(notFoundLocationException);
            }
            catch (DbUpdateConcurrencyException databaseUpdateConcurrencyException)
            {
                var lockedLocationException =
                    new LockedLocationException(databaseUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(lockedLocationException);
            }
            catch (SqlException sqlException)
            {
                var failedStorageException =
                    new FailedLocationStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedStorageException);
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
            var locationValidationException =
                new LocationValidationException(exception);

            this.loggingBroker.LogError(locationValidationException);

            return locationValidationException;
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
