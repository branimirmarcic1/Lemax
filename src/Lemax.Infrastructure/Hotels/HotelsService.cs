using Lemax.Application.Hotels;
using Lemax.Domain;
using Lemax.Infrastructure.Persistence.Context;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemax.Infrastructure.Hotels;

public class HotelsService : IHotelsService
{
    private readonly LemaxDbContext _lemaxDbContext;

    public HotelsService(LemaxDbContext lemaxDbContext)
    {
        _lemaxDbContext = lemaxDbContext;
    }

    public Task<HotelDto> CreateAsync(CreateHotelRequest createHotelRequest, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<List<HotelDto>> GetListAsync(CancellationToken cancellationToken)
    {
        List<Hotel> respone = await _lemaxDbContext.Hotels.ToListAsync(cancellationToken);
        return respone.Adapt<List<HotelDto>>();
    }
}

