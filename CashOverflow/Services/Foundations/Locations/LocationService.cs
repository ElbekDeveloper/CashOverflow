// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CashOverflow.Brokers.Loggings;
using CashOverflow.Brokers.Storages;
using CashOverflow.Models.Locations;
using CashOverflow.Models.Locations.Exceptions;

namespace CashOverflow.Services.Foundations.Locations
{
    public class LocationService : ILocationService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;

        public LocationService(IStorageBroker storageBroker, ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
        }
 

        public async ValueTask<Location> AddLocationAsync(Location location)
        {
            try
            {
                if (location is null)
                {
                    throw new NullLocationException(); 
                }

                return await this.storageBroker.InsertLocationAsync(location);
            }
            catch (NullLocationException nullLocationException)
            {
                var locationValidationException = new LocationValidationException(nullLocationException);
                this.loggingBroker.LogError(locationValidationException);

                throw locationValidationException;
            }
           
        }
    }
}
