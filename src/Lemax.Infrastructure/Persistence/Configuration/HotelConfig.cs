using Lemax.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemax.Infrastructure.Persistence.Configuration;

public class HotelConfig : IEntityTypeConfiguration<Hotel>
{
    public void Configure(EntityTypeBuilder<Hotel> builder)
    {
        builder
            .ToTable(nameof(Hotel), SchemaNames.Inventory);
    }
}