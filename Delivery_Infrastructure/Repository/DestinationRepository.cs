using Delivery_Application_Contracts.Destination;
using Delivery_Domain.DestinationAgg;
using Delivery_Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery_Infrastructure.Repository
{
    public class DestinationRepository : IDestinationRepository
    {
        public List<Destination> destinations;
        private readonly DeliveryContext _context;
        public DestinationRepository(DeliveryContext deliveryContext)
        {
            _context = deliveryContext;
        }
        public Destination Get(int id)
        {
           return _context.Destination.FirstOrDefault(x => x.Id == id);
        }
        public void Create(Destination destination)
        {
            _context.Destination.Add(destination);
            SaveChanges();
        }

        public EditDestination GetEditDetailes(int id)
        {
            return _context.Destination.Select(x => new EditDestination
            {
                Id = x.Id,
                DestinationName = x.DestinationName,
                Price = x.Price,

            }).FirstOrDefault(x => x.Id == id);
        }

        public bool Exist(string name)
        {
            return _context.Destination.Any(x => x.DestinationName == name);
        }

        public List<DestinationViewModel> GetAll()
        {
            return _context.Destination.Select(x => new DestinationViewModel()
            {
                Id = x.Id,
                DestinationName = x.DestinationName,
                Price = x.Price,
                
            }).ToList();
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }

    }
}
