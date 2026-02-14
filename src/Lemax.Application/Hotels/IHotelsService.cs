using Lemax.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemax.Application.Hotels;

public interface IHotelsService : ITransientService
{
    Task<List<HotelDto>> GetListAsync(CancellationToken cancellationToken);
    Task<HotelDto> CreateAsync(CreateHotelRequest createHotelRequest, CancellationToken cancellationToken);
}