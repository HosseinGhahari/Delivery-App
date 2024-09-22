using Delivery_Application_Contracts.Delivery;
using Delivery_Domain.AuthAgg;
using Delivery_Domain.DeliveryAgg;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;
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
        private readonly UserManager<User> _userManager;
        public DeliveryApplication(IDeliveryRepository deliveryRepository , UserManager<User> usermanager)
        {
            _deliveryRepository = deliveryRepository;
            _userManager = usermanager;
        }


        // Creates a new delivery with the given command,
        // adds it to the repository, and saves the changes
        public async Task CreateAsync(CreateDelivery command)
        {
            var delivery = new Delivery(command.IsPaid, command.DestinationId, command.DeliveryTime , command.UserId);
            await _deliveryRepository.CreateAsync(delivery);
            await _deliveryRepository.SaveChangesAsync();
        }

        // Edits an existing Delivery with the given command.
        // Retrieves the Delivery from the repository using its id.
        // If the Delivery doesn't exist, throws an exception.
        // If it does, edits the destination and saves the changes to the repository.
        // also we edit destination from destination table using DestinationId
        public async Task EditAsync(EditDelivery command)
        {
            var delivery = await _deliveryRepository.GetAsync(command.Id);

            if (delivery == null)
                throw new Exception("Delivery not found");

            delivery.Edit(command.IsPaid, command.DestinationId, command.DeliveryTime);
            await _deliveryRepository.SaveChangesAsync();
        }

        // Retrieves the details of a destination for editing.
        // The destination is identified by its id.
        public async Task<EditDelivery> GetEditDetailsAsync(int id)
        {
            return await _deliveryRepository.GetEditDetailsAsync(id);
        }

        // Retrieves the montly income
        public async Task<List<InComeViewModel>> GetInComeAsync()
        {
            return await _deliveryRepository.GetInComeAsync();
        }

        // here in our application layer we retrieves the 
        // all the paid from delivery and for that it uses 
        // method of the Delivery Repository
        public async Task<double> GetNotPaidPriceAsync()
        {
            return await _deliveryRepository.GetNotPaidPriceAsync();
        }

        // here in our application layer we retrieves the 
        // all the Notpaid prices from delivery and for that it uses 
        // method of the Delivery Repository
        public async Task<double> GetPaidPriceAsync()
        {
            return await _deliveryRepository.GetPaidPriceAsync();
        }

        // This method iterates through all delivery records that have
        // not been paid and are not marked as removed. It updates the
        // 'IsPaid' status of each qualifying delivery to true, effectively
        // processing the payment. This is typically called during the checkout process.
        public async Task MarkAllAsPaidAsync()
        {
            var deliveries = await _deliveryRepository.GetPaymentsAsync();
            foreach (var item in deliveries)
            {
                item.IsPaid = true;
            }
            await _deliveryRepository.SaveChangesAsync();
        }

        // This method removes a delivery record by its ID. 
        // It retrieves the record, marks it as removed,
        // and saves the changes in the repository.
        public async Task RemoveAsync(int id)
        {
            var delivery = await _deliveryRepository.GetAsync(id);

            if (delivery == null)
                throw new Exception("Delivery not found");

            delivery.Remove();
            await _deliveryRepository.SaveChangesAsync();
        }

        public async Task<List<DeliveryViewModel>> SearchAsync(string command , string userId)
        {
           if(string.IsNullOrWhiteSpace(userId))
                throw new UnauthorizedAccessException("User is not authenticated.");

            return await _deliveryRepository.SearchAsync(command , userId);
        }

    }
}
