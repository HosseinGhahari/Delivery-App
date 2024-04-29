using Delivery_Application_Contracts.Delivery;
using Delivery_Domain.DeliveryAgg;
using Delivery_Domain.DestinationAgg;
using Delivery_Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery_Infrastructure.Repository
{
    public class DeliveryRepository : IDeliveryRepository
    {
        private readonly DeliveryContext _context;
        public DeliveryRepository(DeliveryContext deliveryContext)
        {
            _context = deliveryContext;
        }
        public void Create(Delivery createDelivery)
        {
            _context.Delivery.Add(createDelivery);
            SaveChanges();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
