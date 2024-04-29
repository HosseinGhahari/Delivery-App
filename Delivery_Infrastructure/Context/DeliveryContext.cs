using Delivery_Domain.DeliveryAgg;
using Delivery_Domain.DestinationAgg;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery_Infrastructure.Context
{
    public class DeliveryContext : DbContext
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
