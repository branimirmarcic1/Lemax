using Lemax.Domain;
using Lemax.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemax.Infrastructure.Persistence.Initialization;

internal class LemaxDbSeeder
{
    public async Task SeedDatabaseAsync(LemaxDbContext dbContext)
    {
        await SeedHotelsAsync(dbContext);
    }

    private async Task SeedHotelsAsync(LemaxDbContext dbContext)
    {
        if (await dbContext.Hotels.AnyAsync())
        {
            return;
        }

        List<Hotel> hotels = new List<Hotel>
        {
            new Hotel
            {
                Name = "Esplanade Zagreb Hotel",
                Price = 180.50m,
                Latitude = 45.8054,
                Longitude = 15.9760
            },
            
            new Hotel
            {
                Name = "Hotel Dubrovnik",
                Price = 110.00m,
                Latitude = 45.8130,
                Longitude = 15.9760
            },

            new Hotel
            {
                Name = "DoubleTree by Hilton",
                Price = 145.00m,
                Latitude = 45.8009,
                Longitude = 15.9960
            },

            new Hotel
            {
                Name = "Hotel Sliško",
                Price = 65.00m,
                Latitude = 45.8045,
                Longitude = 15.9922
            },

            new Hotel
            {
                Name = "Hotel Antunović",
                Price = 105.00m,
                Latitude = 45.7958,
                Longitude = 15.9038
            },

            new Hotel
            {
                Name = "Hotel I",
                Price = 55.00m,
                Latitude = 45.7744,
                Longitude = 15.9558
            },

            new Hotel
            {
                Name = "Hotel Puntijar",
                Price = 130.00m,
                Latitude = 45.8450,
                Longitude = 15.9850
            },

            new Hotel
            {
                Name = "Hilton Garden Inn",
                Price = 125.00m,
                Latitude = 45.8020,
                Longitude = 15.9980
            },

            new Hotel
            {
                Name = "Swanky Mint Hostel",
                Price = 35.00m,
                Latitude = 45.8133,
                Longitude = 15.9688
            },

            new Hotel
            {
                Name = "Hotel Phoenix",
                Price = 75.00m,
                Latitude = 45.8270,
                Longitude = 16.1080
            }
        };

        dbContext.Hotels.AddRange(hotels);
        await dbContext.SaveChangesAsync();
    }
}
