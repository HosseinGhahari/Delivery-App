using Delivery_Application_Contracts.Destination;
using Delivery_Domain.DestinationAgg;
using Delivery_Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
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
        public async Task<Destination> GetAsync(int id)
        {
            return await _context.Destination.FirstOrDefaultAsync(x => x.Id == id);
        }


        // This method create a new Destination object 
        public async Task CreateAsync(Destination destination)
        {
            await _context.Destination.AddAsync(destination);
            await SaveChangesAsync();
        }


        // This method is utilized for obtaining the
        // details of our object for editing purposes.
        // It retrieves the current state of the object,
        // but does not directly apply any edits.
        public async Task<EditDestination> GetEditDetailsAsync(int id)
        {
            return await _context.Destination
                .Select(x => new EditDestination
                {
                    Id = x.Id,
                    DestinationName = x.DestinationName,
                    Price = x.Price,
                })
                .FirstOrDefaultAsync(x => x.Id == id);
        }


        // This method is utilized to verify whether a given
        // 'destinationName' exists in the database.
        public async Task<bool> ExistAsync(string name)
        {
            return await _context.Destination.AnyAsync(x => x.DestinationName == name);
        }


        //This method get all Destination objects from database
        public async Task<List<DestinationViewModel>> GetAllAsync(string userId)
        {
            return await _context.Destination
                .Where(x => x.UserId == userId)
                .Select(dest => new DestinationViewModel 
                {
                    Id = dest.Id,
                    DestinationName = dest.DestinationName,
                    Price = dest.Price,
                })
                .ToListAsync();
        }


        // This method Save the Changes in databasse 
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
