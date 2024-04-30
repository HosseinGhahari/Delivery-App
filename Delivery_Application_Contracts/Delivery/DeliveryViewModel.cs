using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery_Application_Contracts.Delivery
{
    public class DeliveryViewModel
    {
        public int Id { get; set; }
        public DateTime DeliveryTime { get; set; }
        public string PersianDeliveryTime { get; set; }
        public bool IsPaid { get; set; }
        public double Price { get; set; }
        public string Destination { get; set; }

    }
}
