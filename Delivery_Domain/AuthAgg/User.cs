using Delivery_Domain.DeliveryAgg;
using Delivery_Domain.DestinationAgg;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery_Domain.AuthAgg
{
    // This class extends IdentityUser to add 'CustomField', 'IsRemoved', 
    // and a collection of 'Deliveries'. It supports soft delete via the 
    // 'Remove' method and allows updating 'CustomField' while keeping 
    // the logic encapsulated.
    public class User : IdentityUser
    {
        public string CustomField { get; private set; }

        public ICollection<Delivery> Deliveries { get; set; }
        public ICollection<Destination> Destinations { get; set; }
        public User()
        {
            Deliveries = new List<Delivery>();
            Destinations = new List<Destination>();
        }
    }

}
