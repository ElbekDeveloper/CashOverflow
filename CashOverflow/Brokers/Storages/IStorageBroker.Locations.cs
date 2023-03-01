// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using CashOverflow.Models.Locations;

namespace CashOverflow.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Location> InsertLocationAsync(Location location);
        IQueryable<Location> SelectAllLocations();
        ValueTask<Location> SelectLocationByIdAsync(Guid Id);
        ValueTask<Location> DeleteLocationAsync(Location location);
    }
}
