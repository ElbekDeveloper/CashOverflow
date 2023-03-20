// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using CashOverflow.Models.Locations;
using CashOverflow.Models.Salaries;
using Microsoft.EntityFrameworkCore;

namespace CashOverflow.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Salary> Locations { get; set; }

        public async ValueTask<Salary> InsertLocationAsync(Salary location) =>
            await InsertAsync(location);

        public IQueryable<Salary> SelectAllLocations() =>
         SelectAll<Salary>();

        public async ValueTask<Salary> SelectLocationByIdAsync(Guid id) =>
           await SelectAsync<Salary>(id);

        public async ValueTask<Salary> UpdateLocationAsync(Salary location) =>
            await UpdateAsync(location);

        public async ValueTask<Salary> DeleteLocationAsync(Salary location) =>
            await DeleteAsync<Salary>(location);
    }
}
