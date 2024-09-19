using Delivery_Domain.AuthAgg;
using Delivery_Domain.DeliveryAgg;
using Delivery_Domain.DestinationAgg;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Delivery_Infrastructure.Context
{
    // In this section, we establish DbSet properties for our entities.
    // Additionally, we utilize the assembly and the OnModelCreating
    // method to enable our application to recognize all our model mappings.

    public class DeliveryContext : IdentityDbContext
    {
        public DbSet<Delivery> Delivery { get; set; }
        public DbSet<Destination> Destination { get; set; }
        public DeliveryContext(DbContextOptions<DeliveryContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var assembly = typeof(DeliveryContext).Assembly;
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);
            base.OnModelCreating(modelBuilder);
        }

    }
}
