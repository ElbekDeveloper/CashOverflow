// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Threading.Tasks;
using CashOverflow.Models.Jobs.Exceptions;
using CashOverflow.Models.Locations;
using CashOverflow.Models.Locations.Exceptions;
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
            catch (NullLocationException nullLocationException)
            {
                throw CreateAndLogValidationException(nullLocationException);
            }
            catch(InvalidLocationException invalidLocationException)
            {
                throw CreateAndLogValidationException(invalidLocationException);
            }
            catch(SqlException sqlException)
            {
                var failedLocationStorageException = new FailedLocationStorageException(sqlException);

                throw CreateAndLogCriticalDependancyException(failedLocationStorageException);
            }
        }

        private LocationValidationException CreateAndLogValidationException(Xeption exception)
        {
            var locationValidationException = new LocationValidationException(exception);
            this.loggingBroker.LogError(locationValidationException);

            return locationValidationException;
        }

        private LocationDependancyException CreateAndLogCriticalDependancyException(Xeption exception)
        {
            var locationDependancyException = new LocationDependancyException(exception); 
            this.loggingBroker.LogCritical(locationDependancyException);

            return locationDependancyException;
        }
    }
}