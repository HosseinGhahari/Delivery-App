using Delivery_Application_Contracts.Delivery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery_Domain.DeliveryAgg
{
    public interface IDeliveryRepository
    {
        void Create(Delivery createDelivery);
        void SaveChanges();
    }
}
