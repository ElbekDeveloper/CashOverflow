// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using CashOverflow.Models.Locations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
<<<<<<< HEAD
using CashOverflow.Models.Locations;
using Microsoft.EntityFrameworkCore;
=======
>>>>>>> cd307bc66517cf430e3e32d96cdfcc10dbe2ca48

namespace CashOverflow.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Location> Locations { get; set; }

        public async ValueTask<Location> InsertLocationAsync(Location location) =>
            await InsertAsync(location);

        public IQueryable<Location> SelectAllLocations() =>
         SelectAll<Location>();

        public async ValueTask<Location> SelectLocationByIdAsync(Guid id) =>
           await SelectAsync<Location>(id);

        public async ValueTask<Location> UpdateLocationAsync(Location location) =>
            await UpdateAsync(location);

        public async ValueTask<Location> DeleteLocationAsync(Location location) =>
            await DeleteAsync<Location>(location);
    }
}
