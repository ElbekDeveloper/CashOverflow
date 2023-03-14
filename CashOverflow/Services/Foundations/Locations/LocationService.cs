// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Threading.Tasks;
using CashOverflow.Brokers.DateTimes;
using CashOverflow.Brokers.Loggings;
using CashOverflow.Brokers.Storages;
using CashOverflow.Models.Locations;

namespace CashOverflow.Services.Foundations.Locations
{
    public partial class LocationService : ILocationService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public LocationService(
            ILoggingBroker loggingBroker,
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.loggingBroker = loggingBroker;
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public ValueTask<Location> RemoveLocationByIdAsync(Guid locationId) =>
            TryCatch(async () =>
            {
                ValidateLocationById(locationId);
                Location someLocation = await this.storageBroker.SelectLocationByIdAsync(locationId);

                return await this.storageBroker.DeleteLocationAsync(someLocation);
            });
    }
}
