using Delivery_Application_Contracts.Delivery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery_Domain.DeliveryAgg
{
    // In this section, we establish the interface for the Delivery Repository.
    // The implementation of these interfaces is carried out in their respective
    // repositories within the Infrastructure layer.

    public interface IDeliveryRepository
    {
        Task CreateAsync(Delivery createDelivery);
        Task<List<Delivery>> GetPaymentsAsync();
        Task<Delivery> GetAsync(int id);
        Task<EditDelivery> GetEditDetailsAsync(int id);
        Task<double> GetPaidPriceAsync();
        Task<double> GetNotPaidPriceAsync();
        Task SaveChangesAsync();
        Task<List<DeliveryViewModel>> SearchAsync(string search , string userId);
        Task<List<InComeViewModel>> GetInComeAsync();
    }

}
