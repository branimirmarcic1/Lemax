using Lemax.Application.Hotels;
using Lemax.Domain;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemax.Infrastructure.Mapping;

public class MapsterSettings
{
    public static void Configure()
    {
        // here we will define the type conversion / Custom-mapping
        // More details at https://github.com/MapsterMapper/Mapster/wiki/Custom-mapping

        TypeAdapterConfig<Hotel, HotelDto>.NewConfig();
        TypeAdapterConfig<CreateHotelRequest, Hotel>.NewConfig();
    }
}