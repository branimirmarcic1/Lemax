using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemax.Application.Hotels;

public class SearchHotelRequest
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}