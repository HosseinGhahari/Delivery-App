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

    // This section pertains to our Destination Repository, where
    // we perform the primary query operations on the database.
    // To facilitate these operations,we inject our context.

    public class DestinationRepository : IDestinationRepository
    {
        public List<Destination> destinations;
        private readonly DeliveryContext _context;
        public DestinationRepository(DeliveryContext deliveryContext)
        {
            _context = deliveryContext;
        }

        
        // this method get a Destination object by its id for future use
        public Destination Get(int id)
        {
           return _context.Destination.FirstOrDefault(x => x.Id == id);
        }


        // This method create a new Destination object 
        public void Create(Destination destination)
        {
            _context.Destination.Add(destination);
            SaveChanges();
        }


        // This method is utilized for obtaining the
        // details of our object for editing purposes.
        // It retrieves the current state of the object,
        // but does not directly apply any edits.
        public EditDestination GetEditDetailes(int id)
        {
            return _context.Destination.Select(x => new EditDestination
            {
                Id = x.Id,
                DestinationName = x.DestinationName,
                Price = x.Price,

            }).FirstOrDefault(x => x.Id == id);
        }


        // This method is utilized to verify whether a given
        // 'destinationName' exists in the database.
        public bool Exist(string name)
        {
            return _context.Destination.Any(x => x.DestinationName == name);
        }


        //This method get all Destination objects from database
        public List<DestinationViewModel> GetAll()
        {
            return _context.Destination.Select(x => new DestinationViewModel()
            {
                Id = x.Id,
                DestinationName = x.DestinationName,
                Price = x.Price,
                
            }).ToList();
        }


        // This method Save the Changes in databasse 
        public void SaveChanges()
        {
            _context.SaveChanges();
        }

    }
}
