using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemax.Application.Hotels
{
    public class HotelSearchResultDto
    {
        public required string Name { get; set; }
        public decimal Price { get; set; }

        public double DistanceKM { get; set; }
    }
}
