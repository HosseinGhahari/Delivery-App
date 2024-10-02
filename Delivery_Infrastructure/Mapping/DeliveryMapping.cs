using Delivery_Domain.DeliveryAgg;
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

    public class DeliveryMapping : IEntityTypeConfiguration<Delivery>
    {
        public void Configure(EntityTypeBuilder<Delivery> builder)
        {
            builder.ToTable("Delivery");
            builder.HasKey(x => x.Id);

            builder
                .HasOne(x => x.Destination)
                .WithMany(x => x.Deliveries)
                .HasForeignKey(x => x.DestinationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(x => x.User)
                .WithMany(x => x.Deliveries)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
