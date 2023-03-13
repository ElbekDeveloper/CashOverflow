// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System.Threading.Tasks;
using CashOverflow.Brokers.DateTimes;
using CashOverflow.Brokers.Loggings;
using CashOverflow.Brokers.Storages;
using CashOverflow.Models.Locations;

namespace CashOverflow.Services.Foundations.Locations {
    public partial class LocationService : ILocationService {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public LocationService(
            IStorageBroker storageBroker,
               IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker) {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Location> AddLocationAsync(Location location) =>
        TryCatch(async () => {
            ValidateLocationOnAdd(location);

            return await this.storageBroker.InsertLocationAsync(location);
        });
    }
}
