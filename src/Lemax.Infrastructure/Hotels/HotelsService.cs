using Lemax.Application.Common.Exceptions;
using Lemax.Application.Common.Models;
using Lemax.Application.Hotels;
using Lemax.Domain;
using Lemax.Infrastructure.Persistence.Context;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Lemax.Infrastructure.Hotels;

public class HotelsService : IHotelsService
{
    private readonly LemaxDbContext _lemaxDbContext;

    public HotelsService(LemaxDbContext lemaxDbContext)
    {
        _lemaxDbContext = lemaxDbContext;
    }

    public async Task<PaginationResponse<HotelDto>> GetListAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var query = _lemaxDbContext.Hotels.AsNoTracking();
        int totalCount = await query.CountAsync(cancellationToken);

        var hotels = await query
            .OrderBy(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var dtos = hotels.Adapt<List<HotelDto>>();

        return new PaginationResponse<HotelDto>(dtos, totalCount, page, pageSize);
    }

    public async Task<HotelDto> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var hotel = await _lemaxDbContext.Hotels
            .AsNoTracking()
            .FirstOrDefaultAsync(h => h.Id == id, cancellationToken);

        if (hotel == null)
        {
            throw new NotFoundException($"Hotel with ID {id} not found");
        }

        return hotel.Adapt<HotelDto>();
    }

    public async Task<HotelDto> CreateAsync(CreateHotelRequest createHotelRequest, CancellationToken cancellationToken)
    {
        bool hotelExists = await _lemaxDbContext.Hotels
            .AnyAsync(h => h.Name == createHotelRequest.Name, cancellationToken);

        if (hotelExists)
        {
            throw new ConflictException($"Hotel with the name '{createHotelRequest.Name}' already exists.");
        }

        var hotel = createHotelRequest.Adapt<Hotel>();

        await _lemaxDbContext.Hotels.AddAsync(hotel);
        await _lemaxDbContext.SaveChangesAsync(cancellationToken);

        return hotel.Adapt<HotelDto>();
    }

    public async Task<HotelDto> UpdateAsync(int id, CreateHotelRequest request, CancellationToken cancellationToken)
    {
        var hotel = await _lemaxDbContext.Hotels.FindAsync(new object[] { id }, cancellationToken);

        if (hotel == null)
        {
            throw new NotFoundException($"Hotel with ID {id} not found");
        }

        request.Adapt(hotel);

        await _lemaxDbContext.SaveChangesAsync(cancellationToken);

        return hotel.Adapt<HotelDto>();
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var hotel = await _lemaxDbContext.Hotels.FindAsync(new object[] { id }, cancellationToken);

        if (hotel == null)
        {
            throw new NotFoundException($"Hotel with ID {id} not found");
        }

        _lemaxDbContext.Hotels.Remove(hotel);
        await _lemaxDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<PaginationResponse<HotelSearchResultDto>> SearchAsync(
        double myLat,
        double myLon,
        int pageIndex,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var hotels = await _lemaxDbContext.Hotels.AsNoTracking().ToListAsync(cancellationToken);

        var query = hotels.Select(h => new HotelSearchResultDto
        {
            Name = h.Name,
            Price = h.Price,
            DistanceKM = CalculateDistance(myLat, myLon, h.Latitude, h.Longitude)
        });

        var sortedQuery = query.OrderBy(x => (double)x.Price + x.DistanceKM).ToList();

        int totalCount = sortedQuery.Count;

        var pagedData = sortedQuery
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new PaginationResponse<HotelSearchResultDto>(pagedData, totalCount, pageIndex, pageSize);
    }

    private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        const double R = 6371;
        var dLat = ToRadians(lat2 - lat1);
        var dLon = ToRadians(lon2 - lon1);

        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return Math.Round(R * c, 2);
    }

    private double ToRadians(double angle) => Math.PI * angle / 180.0;
}

