using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery_Application_Contracts.Delivery
{
    // The IDeliveryApplication interface outlines the operations for
    // a delivery application.It includes methods for creation, editing,
    // conversions and removing , retrieval of all destinations, and fetching details
    // for editing a destination.Each method has specific inputs and outputs.
    // also we take our dependencies from classes in this layer
    public interface IDeliveryApplication
    {
        Task CreateAsync(CreateDelivery command);
        Task RemoveAsync(int id);
        Task<List<DeliveryViewModel>> GetAllAsync();
        Task<EditDelivery> GetEditDetailsAsync(int id);
        Task EditAsync(EditDelivery command);
        Task<double> GetPaidPriceAsync();
        Task<double> GetNotPaidPriceAsync();
        Task MarkAllAsPaidAsync();
        Task<List<DeliveryViewModel>> SearchAsync(string command);
        Task<List<InComeViewModel>> GetInComeAsync();
    }
}
