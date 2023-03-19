// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Linq;
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
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public LocationService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)

        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public ValueTask<Location> AddLocationAsync(Location location) =>
            TryCatch(async () =>
            {
                ValidateLocationOnAdd(location);

                return await this.storageBroker.InsertLocationAsync(location);
            });

        public ValueTask<Location> RetrieveLocationByIdAsync(Guid locationId) =>
        TryCatch(async () =>
        {
            ValidateLocationId(locationId);

            Location maybeLocation =
                await this.storageBroker.SelectLocationByIdAsync(locationId);

            ValidateStorageLocation(maybeLocation, locationId);

<<<<<<< Updated upstream
            return maybeLocation;
        });
=======
                return maybeLocation;
            });

        public IQueryable<Location> RetrieveAllLocations() =>
            TryCatch(() => this.storageBroker.SelectAllLocations());
>>>>>>> Stashed changes
    }
}
