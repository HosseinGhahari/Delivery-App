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
    // This section pertains to our Delivery Repository, where
    // we perform the primary query operations on the database.
    // To facilitate these operations,we inject our context.

    public class DeliveryRepository : IDeliveryRepository
    {
        private readonly DeliveryContext _context;
        public DeliveryRepository(DeliveryContext deliveryContext)
        {
            _context = deliveryContext;
        }


        // This method create a new Destination object 
        public void Create(Delivery createDelivery)
        {
            _context.Delivery.Add(createDelivery);
            SaveChanges();
        }


        // This method Save the Changes in databasse 
        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
