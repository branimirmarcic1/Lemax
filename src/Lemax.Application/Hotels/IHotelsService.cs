using Lemax.Application.Common.Interfaces;
using Lemax.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemax.Application.Hotels;

public interface IHotelsService : ITransientService
{
    Task<PaginationResponse<HotelDto>> GetListAsync(int page, int pageSize, CancellationToken cancellationToken);
    Task<HotelDto> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<HotelDto> CreateAsync(CreateHotelRequest request, CancellationToken cancellationToken);
    Task<HotelDto> UpdateAsync(int id, CreateHotelRequest request, CancellationToken cancellationToken);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
    Task<PaginationResponse<HotelSearchResultDto>> SearchAsync(double myLat,double myLon, int pageIndex, int pageSize, CancellationToken cancellationToken);
}