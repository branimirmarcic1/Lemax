using Lemax.Application.Hotels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemax.Infrastructure.Hotels;

public class HotelsService : IHotelsService
{
    public Task<HotelDto> CreateAsync(CreateHotelRequest createHotelRequest, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<List<HotelDto>> GetListAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

