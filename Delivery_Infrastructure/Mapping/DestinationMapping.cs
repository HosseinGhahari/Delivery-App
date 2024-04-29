using Delivery_Domain.DestinationAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery_Infrastructure.Mapping
{
    public class DestinationMapping : IEntityTypeConfiguration<Destination>
    {
        public void Configure(EntityTypeBuilder<Destination> builder)
        {
            builder.ToTable("Destination");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.DestinationName).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Price).IsRequired();

            builder
                .HasMany(x => x.Deliveries)
                .WithOne(x => x.Destination)
                .HasForeignKey(x => x.DestinationId);
        }
    }
}
