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
        void Create(Delivery createDelivery);
        List<DeliveryViewModel> GetAll();
        List<Delivery> GetPayments();
        Delivery Get(int id);
        EditDelivery GetEditDetailes(int id);
        double GetPaidPrice();
        double GetNotPaidPrice();
        void SaveChanges();
        List<DeliveryViewModel> Search(string search);
        List<InComeViewModel> GetInCome();

    }
}
