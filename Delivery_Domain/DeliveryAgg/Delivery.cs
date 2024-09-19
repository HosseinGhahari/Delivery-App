using Delivery_Domain.AuthAgg;
using Delivery_Domain.DestinationAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery_Domain.DeliveryAgg
{
    // In this section, we define the necessary properties
    // for our Delivery model. We also establish a constructor
    // for the model and implement methods for editing and removal.

    // As this model forms the 'N' part of the database relationship,
    // it includes both 'DestinationID' and 'Destination' properties.

    public class Delivery
    {
        public int Id { get; set; }
        public DateTime DeliveryTime { get; set; }
        public bool IsPaid { get; set; }
        public bool IsRemoved { get; set; }
        public int DestinationId { get; set; }
        public Destination Destination { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }

        public Delivery()
        {

        }
        public Delivery(bool isPaid, int destinationId, DateTime date , string userid)
        {
            IsPaid = isPaid;
            DestinationId = destinationId;
            DeliveryTime = date;
            UserId = userid;
        }

        public void Edit(bool isPaid, int destinationId, DateTime date)
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
