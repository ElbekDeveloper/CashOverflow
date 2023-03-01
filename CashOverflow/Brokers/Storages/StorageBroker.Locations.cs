// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using CashOverflow.Models.Locations;
using Microsoft.EntityFrameworkCore;

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
            await UpdateLocationAsync(location);

      }
}
