using Lemax.Application.Common.Exceptions;
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

    public async Task<List<HotelDto>> GetListAsync(CancellationToken cancellationToken)
    {
        List<Hotel> response = await _lemaxDbContext.Hotels.ToListAsync(cancellationToken);
        return response.Adapt<List<HotelDto>>();
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
}

