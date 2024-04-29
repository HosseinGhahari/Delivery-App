using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery_Application_Contracts.Delivery
{
    public class CreateDelivery
    {
        public DateTime DeliveryTime { get; set; }
        public bool IsPaid { get; set; }
        public bool IsRemoved { get; set; }
        public int DestinationId { get; set; }
    }
}
