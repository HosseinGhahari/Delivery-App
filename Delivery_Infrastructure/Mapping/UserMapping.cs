using Delivery_Domain.AuthAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery_Infrastructure.Mapping
{
    // This class configures the 'User' entity to map to the 'AspNetUsers' table 
    // in the database. It sets constraints for 'UserName' and 'PasswordHash' 
    // with a max length of 256 and makes them required. It also defines a one-to-many 
    // relationship between 'User' and 'Deliveries' with 'UserId' as the foreign key.
    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("AspNetUsers");

            builder.Property(x => x.UserName).HasMaxLength(256).IsRequired();
            builder.Property(x => x.PasswordHash).HasMaxLength(256).IsRequired();

            builder
                .HasMany(x => x.Deliveries)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasMany(x => x.Destinations)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade); 
        }
    }
}
