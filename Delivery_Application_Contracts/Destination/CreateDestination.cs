using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery_Application_Contracts.Destination
{
    // This is the 'Create' object that will be injected
    // into the methods of the 'IDestinationApplication' interface.

    public class CreateDestination
    {
        public string DestinationName { get; set; }
        public double Price { get; set; }
    }
}
