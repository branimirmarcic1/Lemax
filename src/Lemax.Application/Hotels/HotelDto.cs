using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemax.Application.Hotels;

public class HotelDto
{
    public required string Name { get; set; }
    public decimal Price { get; set; }

    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
