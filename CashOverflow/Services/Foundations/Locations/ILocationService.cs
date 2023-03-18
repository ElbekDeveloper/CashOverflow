// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System.Threading.Tasks;
using CashOverflow.Models.Locations;

namespace CashOverflow.Services.Foundations.Locations
{
    public interface ILocationService
    {
        ValueTask<Location> AddLocationAsync(Location location);
        ValueTask<Location> ModifyLocationAsync(Location location);
    }
}
