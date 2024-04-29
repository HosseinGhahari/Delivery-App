using Delivery_Domain.DestinationAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery_Domain.DeliveryAgg
{
    public class Delivery
    {
        public int Id { get; set; }
        public DateTime DeliveryTime { get; set; }
        public bool IsPaid { get; set; }
        public bool IsRemoved { get; set; }
        public int DestinationId { get; set; }
        public Destination Destination { get; set; }
        public Delivery()
        {
            
        }
        public Delivery(bool isPaid, int destinationId , DateTime date) 
        {  
            IsPaid = isPaid;
            DestinationId = destinationId;
            DeliveryTime = date;
        }

        public void Edit(bool isPaid, int destinationId , DateTime date)
        {
            IsPaid = isPaid;
            DestinationId = destinationId;
            DeliveryTime = date;
        }


        public void Remove()
        {
            IsRemoved = true;
        }
    }

}
