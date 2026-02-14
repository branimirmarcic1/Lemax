using FluentAssertions;
using Lemax.Application.Common.Exceptions;
using Lemax.Application.Hotels;
using Lemax.Domain;
using Lemax.Infrastructure.Hotels;
using Lemax.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Lemax.UnitTests.Hotels;

public class HotelsServiceTests : IDisposable
{
    private readonly LemaxDbContext _dbContext;
    private readonly HotelsService _hotelsService;

    public HotelsServiceTests()
    {
        // Kreiramo novu bazu za svaki test kako se podaci ne bi miješali
        var options = new DbContextOptionsBuilder<LemaxDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new LemaxDbContext(options);
        _hotelsService = new HotelsService(_dbContext);
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    // Helper za kreiranje hotela s ID-em (ako je Id protected set)
    private Hotel CreateHotel(int id, string name, decimal price, double lat, double lon)
    {
        var hotel = new Hotel
        {
            Name = name,
            Price = price,
            Latitude = lat,
            Longitude = lon
        };

        // Postavljanje ID-a preko refleksije
        var idProp = typeof(Hotel).GetProperty("Id")
                     ?? typeof(Hotel).BaseType?.GetProperty("Id");
        idProp?.SetValue(hotel, id);

        return hotel;
    }

    [Fact]
    public async Task CreateAsync_ShouldAddHotel_WhenDataIsValid()
    {
        // Arrange
        var request = new CreateHotelRequest { Name = "Test Hotel", Price = 100, Latitude = 45, Longitude = 15 };

        // Act
        var result = await _hotelsService.CreateAsync(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Test Hotel");
        _dbContext.Hotels.Count().Should().Be(1);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowConflict_WhenNameAlreadyExists()
    {
        // Arrange
        _dbContext.Hotels.Add(CreateHotel(1, "Existing", 100, 45, 15));
        await _dbContext.SaveChangesAsync();

        var request = new CreateHotelRequest
        {
            Name = "Existing",
            Price = 100,
            Latitude = 45,
            Longitude = 15
        };

        // Act
        Func<Task> act = async () => await _hotelsService.CreateAsync(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ConflictException>();
    }

    [Fact]
    public async Task SearchAsync_ShouldReturnSortedResults_ByPriceAndDistance()
    {
        // Arrange
        // Pozicija korisnika: 45.8, 15.9 (Zagreb)
        var myLat = 45.8060;
        var myLon = 15.9780;

        _dbContext.Hotels.AddRange(
            // Skup i blizu (Score: 180 + 0.17 = 180.17)
            CreateHotel(1, "Esplanade", 180.00m, 45.8054, 15.9760),
            // Jeftin i malo dalje (Score: 35 + 1.08 = 36.08)
            CreateHotel(2, "Swanky", 35.00m, 45.8133, 15.9688)
        );
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _hotelsService.SearchAsync(myLat, myLon, 1, 10, CancellationToken.None);

        // Assert
        // Swanky mora biti prvi jer je Score (36.08) manji od Esplanade (180.17)
        result.Data.First().Name.Should().Be("Swanky");
        result.TotalCount.Should().Be(2);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrowNotFound_WhenIdIsInvalid()
    {
        // Act
        Func<Task> act = async () => await _hotelsService.GetByIdAsync(999, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveHotel_WhenHotelExists()
    {
        // Arrange
        _dbContext.Hotels.Add(CreateHotel(1, "Delete Me", 100, 45, 15));
        await _dbContext.SaveChangesAsync();

        // Act
        await _hotelsService.DeleteAsync(1, CancellationToken.None);

        // Assert
        _dbContext.Hotels.Should().BeEmpty();
    }

    [Fact]
    public async Task GetListAsync_ShouldReturnPaginatedData()
    {
        // Arrange
        _dbContext.Hotels.AddRange(
            CreateHotel(1, "H1", 100, 45, 15),
            CreateHotel(2, "H2", 200, 45, 15)
        );
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _hotelsService.GetListAsync(1, 10, CancellationToken.None);

        // Assert
        result.Data.Should().HaveCount(2);
        result.TotalCount.Should().Be(2);
    }

    [Fact]
    public async Task UpdateAsync_ShouldModifyExistingHotel()
    {
        // Arrange
        var hotel = CreateHotel(1, "Old Name", 100, 45, 15);
        _dbContext.Hotels.Add(hotel);
        await _dbContext.SaveChangesAsync();

        var request = new CreateHotelRequest { Name = "New Name", Price = 150, Latitude = 45, Longitude = 15 };

        // Act
        await _hotelsService.UpdateAsync(1, request, CancellationToken.None);

        // Assert
        var updated = await _dbContext.Hotels.FindAsync(1);
        updated?.Name.Should().Be("New Name");
        updated?.Price.Should().Be(150);
    }
}