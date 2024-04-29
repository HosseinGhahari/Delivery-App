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
    public class DeliveryMapping : IEntityTypeConfiguration<Delivery>
    {
        public void Configure(EntityTypeBuilder<Delivery> builder)
        {
            builder.ToTable("Delivery");
            builder.HasKey(x => x.Id);

            builder
                .HasOne(x => x.Destination)
                .WithMany(x => x.Deliveries)
                .HasForeignKey(x => x.DestinationId);
        }
    }
}
