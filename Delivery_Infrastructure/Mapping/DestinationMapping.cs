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
    // In this section, we configure the mapping for the Delivery model.
    // This includes defining the primary key and establishing the relationships
    // between the various tables in our database.

    // Additionally, within our context, any class that inherits from
    // IEntityTypeConfiguration is identified as a mapping class.
    public class DestinationMapping : IEntityTypeConfiguration<Destination>
    {
        public void Configure(EntityTypeBuilder<Destination> builder)
        {
            builder.ToTable("Destination");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.DestinationName).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Price).IsRequired();
            builder.Property(x => x.UserId);

            builder
                .HasMany(x => x.Deliveries)
                .WithOne(x => x.Destination)
                .HasForeignKey(x => x.DestinationId);
        }
    }
}
