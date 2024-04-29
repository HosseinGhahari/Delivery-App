using Delivery_Domain.DeliveryAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery_Domain.DestinationAgg
{
    public class Destination
    {
        public int Id { get; set; }
        public string DestinationName { get; set; }
        public double Price { get; set; }
        public List<Delivery> Deliveries { get; set; }

        public Destination(string destinationName, double price)
        {
            DestinationName = destinationName;
            Price = price;
            Deliveries = new List<Delivery>();
        }

        public void Edit(string destinationName, double price)
        {
            DestinationName = destinationName;
            Price = price;
        }

    }
}
