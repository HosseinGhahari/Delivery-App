using Delivery_Application_Contracts.Delivery;
using Delivery_Domain.DeliveryAgg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Delivery_Application
{
    // Here, we inject our interface from the Infrastructure layer
    // and inherit from 'IDeliveryApplication' to perform the primary operations.
    // also provides a clear and concise explanation of how the interface from
    // the Infrastructure layer is being used and the role of ‘IDeliveryApplication’

    public class DeliveryApplication : IDeliveryApplication
    {
      
        private readonly IDeliveryRepository _deliveryRepository;
        public DeliveryApplication(IDeliveryRepository deliveryRepository)
        {
            _deliveryRepository = deliveryRepository;
        }


        // Creates a new delivery with the given command,
        // adds it to the repository, and saves the changes
        public void Create(CreateDelivery command)
        {
            var delivery = new Delivery(command.IsPaid,command.DestinationId , command.DeliveryTime);
            _deliveryRepository.Create(delivery);
            _deliveryRepository.SaveChanges();
        }

        // Get All Delivery Values 
        public List<DeliveryViewModel> GetAll()
        {
           return _deliveryRepository.GetAll();
        }


    }
}
