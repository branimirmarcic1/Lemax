using Lemax.Application.Hotels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace Lemax.API.Controllers.Hotel;

[OpenApiTag("Hotel", Description = "")]
[Route("api/[controller]")]
public sealed class HotelsController : BaseApiController
{
    private readonly IHotelsService _hotelsService;

    public HotelsController(IHotelsService hotelsService)
    {
        _hotelsService = hotelsService;
    }

    [HttpGet]
    [OpenApiOperation("Dohvatite listu hotela.", "")]
    public async Task<List<HotelDto>> GetListAsync(CancellationToken cancellationToken)
    {
        return await _hotelsService.GetListAsync(cancellationToken);
    }

    [HttpPost]
    [OpenApiOperation("Dodajte novi hotel.", "")]
    public async Task<HotelDto> CreateAsync(CreateHotelRequest createHotelRequest, CancellationToken cancellationToken)
    {
        return await _hotelsService.CreateAsync(createHotelRequest, cancellationToken);
    }
}
