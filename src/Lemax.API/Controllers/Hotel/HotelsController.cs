using Lemax.Application.Common.Models;
using Lemax.Application.Hotels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace Lemax.API.Controllers.Hotel;

[OpenApiTag("Hotel", Description = "Upravljanje podacima o hotelima.")]
[Route("api/[controller]")]
public sealed class HotelsController : BaseApiController
{
    private readonly IHotelsService _hotelsService;

    public HotelsController(IHotelsService hotelsService)
    {
        _hotelsService = hotelsService;
    }

    [HttpGet]
    [OpenApiOperation("Dohvatite paginiranu listu svih hotela.", "")]
    public async Task<ActionResult<PaginationResponse<HotelDto>>> GetListAsync(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var result = await _hotelsService.GetListAsync(page, pageSize, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id}")]
    [OpenApiOperation("Dohvatite jedan hotel po ID-u.", "")]
    public async Task<HotelDto> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _hotelsService.GetByIdAsync(id, cancellationToken);
    }

    [HttpPost]
    [OpenApiOperation("Dodajte novi hotel.", "")]
    public async Task<HotelDto> CreateAsync([FromBody] CreateHotelRequest createHotelRequest, CancellationToken cancellationToken)
    {
        return await _hotelsService.CreateAsync(createHotelRequest, cancellationToken);
    }

    [HttpPut("{id}")]
    [OpenApiOperation("Ažurirajte postojeći hotel.", "")]
    public async Task<HotelDto> UpdateAsync(int id, [FromBody] CreateHotelRequest request, CancellationToken cancellationToken)
    {
        return await _hotelsService.UpdateAsync(id, request, cancellationToken);
    }

    [HttpDelete("{id}")]
    [OpenApiOperation("Obrišite hotel.", "")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        await _hotelsService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }

    [HttpGet("search")]
    [OpenApiOperation("Pretraga hotela po lokaciji.", "Vraća paginiranu listu hotela.")]
    public async Task<ActionResult<PaginationResponse<HotelSearchResultDto>>> SearchAsync(
            [FromQuery] SearchHotelRequest request,
            CancellationToken cancellationToken)
    {
        var result = await _hotelsService.SearchAsync(
            request.Latitude,
            request.Longitude,
            request.Page,
            request.PageSize,
            cancellationToken);

        return Ok(result);
    }
}