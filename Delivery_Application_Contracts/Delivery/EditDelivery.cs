using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery_Application_Contracts.Delivery
{
    // This is the 'Create' object that will be injected
    // into the methods of the 'IDeliveryApplication' interface.
    // for this part we added DestinationId and DestinationName
    // for us to be able to edit DestinationName from Destination Table
    public class EditDelivery : CreateDelivery
    {
        public int Id { get; set; }
        public int DestinationId { get; set; }
        public string DestinationName { get; set; }
    }
}
