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

        // Edits an existing Delivery with the given command.
        // Retrieves the Delivery from the repository using its id.
        // If the Delivery doesn't exist, throws an exception.
        // If it does, edits the destination and saves the changes to the repository.
        // also we edit destination from destination table using DestinationId

        public void Edit(EditDelivery command)
        {
            var delivery = _deliveryRepository.Get(command.Id);

            if (delivery == null)
                throw new Exception();

            delivery.Edit(command.IsPaid ,command.DestinationId, command.DeliveryTime);
            _deliveryRepository.SaveChanges();
        }

        // Get All Delivery Values 
        public List<DeliveryViewModel> GetAll()
        {
           return _deliveryRepository.GetAll();
        }

        // Retrieves the details of a destination for editing.
        // The destination is identified by its id.
        public EditDelivery GetEditDetailes(int id)
        {
           return _deliveryRepository.GetEditDetailes(id);
        }


        // This method removes a delivery record by its ID. 
        // It retrieves the record, marks it as removed,
        // and saves the changes in the repository.
        public void Remove(int id)
        {
            var delivery = _deliveryRepository.Get(id);

            if (delivery == null)
                throw new Exception();

            delivery.Remove();
            _deliveryRepository.SaveChanges();
        }
    }
}
