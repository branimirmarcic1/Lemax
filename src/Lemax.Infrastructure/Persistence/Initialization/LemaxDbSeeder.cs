using Lemax.Domain;
using Lemax.Infrastructure.Persistence.Context;
using Lemax.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Diagnostics.CodeAnalysis;

namespace Lemax.Infrastructure.Persistence.Initialization;

[ExcludeFromCodeCoverage]
internal class LemaxDbSeeder
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public LemaxDbSeeder(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task SeedDatabaseAsync(LemaxDbContext dbContext)
    {
        await SeedHotelsAsync(dbContext);
        await SeedUsersAsync(_userManager, _roleManager);
    }

    private async Task SeedHotelsAsync(LemaxDbContext dbContext)
    {
        if (await dbContext.Hotels.AnyAsync())
        {
            return;
        }

        List<Hotel> hotels = new List<Hotel>
        {
            // --- CENTAR GRADA (Skupi i Jeftini) ---
            new Hotel { Name = "Esplanade Zagreb Hotel", Price = 180.50m, Latitude = 45.8054, Longitude = 15.9760 }, // Skup, Centar
            new Hotel { Name = "Hotel Dubrovnik", Price = 110.00m, Latitude = 45.8130, Longitude = 15.9760 }, // Srednji, Trg
            new Hotel { Name = "Swanky Mint Hostel", Price = 35.00m, Latitude = 45.8133, Longitude = 15.9688 }, // Jeftin, Ilica
            new Hotel { Name = "Sheraton Zagreb", Price = 150.00m, Latitude = 45.8066, Longitude = 15.9860 }, // Skup, Draškovićeva
            new Hotel { Name = "Canopy by Hilton", Price = 135.00m, Latitude = 45.8038, Longitude = 15.9839 }, // Branimir centar
            new Hotel { Name = "Chillout Hostel", Price = 30.00m, Latitude = 45.8140, Longitude = 15.9700 }, // Najjeftiniji u centru
            new Hotel { Name = "Best Western Astoria", Price = 85.00m, Latitude = 45.8060, Longitude = 15.9810 }, // Best buy centar

            // --- ŠIRI CENTAR / POSLOVNA ZONA ---
            new Hotel { Name = "DoubleTree by Hilton", Price = 145.00m, Latitude = 45.8009, Longitude = 15.9960 }, // Vukovarska
            new Hotel { Name = "Hotel Sliško", Price = 65.00m, Latitude = 45.8045, Longitude = 15.9922 }, // Autobusni
            new Hotel { Name = "Hilton Garden Inn", Price = 125.00m, Latitude = 45.8020, Longitude = 15.9980 }, // Radnička
            new Hotel { Name = "Hotel 9", Price = 95.00m, Latitude = 45.8010, Longitude = 15.9900 }, // Držićeva
            new Hotel { Name = "Livris Hotel", Price = 80.00m, Latitude = 45.8015, Longitude = 15.9999 }, // Raposka

            // --- NOVI ZAGREB (Južno od Save) ---
            new Hotel { Name = "Hotel I", Price = 55.00m, Latitude = 45.7744, Longitude = 15.9558 }, // Blizu Arene
            new Hotel { Name = "Hotel Sundial", Price = 70.00m, Latitude = 45.7700, Longitude = 15.9400 }, // Lanište

            // --- ZAPADNI DIO ---
            new Hotel { Name = "Hotel Antunović", Price = 105.00m, Latitude = 45.7958, Longitude = 15.9038 }, // Zagrebačka avenija
            new Hotel { Name = "Hotel Vienna", Price = 60.00m, Latitude = 45.7920, Longitude = 15.9000 }, // Rudeš

            // --- ISTOČNI DIO / PERIFERIJA (Jako daleko) ---
            new Hotel { Name = "Hotel Phoenix", Price = 75.00m, Latitude = 45.8270, Longitude = 16.1080 }, // Sesvete (Daleko)
            new Hotel { Name = "Hotel Gallus", Price = 70.00m, Latitude = 45.8200, Longitude = 16.0500 }, // Dubrava
            new Hotel { Name = "Kezerle Guest House", Price = 45.00m, Latitude = 45.8100, Longitude = 16.1500 }, // Dugo Selo (Jako daleko, jeftino)

            // --- SJEVER (Podsljeme) ---
            new Hotel { Name = "Hotel Puntijar", Price = 130.00m, Latitude = 45.8450, Longitude = 15.9850 } // Gračani
        };

        dbContext.Hotels.AddRange(hotels);
        await dbContext.SaveChangesAsync();
    }

    public async Task SeedUsersAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        if (!await roleManager.RoleExistsAsync(AdminAccount.Role))
        {
            await roleManager.CreateAsync(new IdentityRole(AdminAccount.Role));
        }

        if (await userManager.FindByEmailAsync(AdminAccount.Email) == null)
        {
            var user = new IdentityUser { UserName = AdminAccount.Email, Email = AdminAccount.Email };
            await userManager.CreateAsync(user, AdminAccount.Password);
            await userManager.AddToRoleAsync(user, AdminAccount.Role);
        }
    }
}