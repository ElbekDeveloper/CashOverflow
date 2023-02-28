// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System.Threading.Tasks;
using CashOverflow.Models.Locations;

namespace CashOverflow.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Location> InsertLocationAsync(Location location);
        ValueTask<Location> SelectLocationByIdAsync(Guid Id);
    }
}
