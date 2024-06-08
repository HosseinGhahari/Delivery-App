using Delivery_Application_Contracts.Delivery;
using Delivery_Domain.DeliveryAgg;
using Delivery_Domain.DestinationAgg;
using Delivery_Infrastructure.Context;
using Delivery_Infrastructure.DateConversionService;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        private readonly IDateConversionService _dateConversionService;

        public DeliveryRepository(DeliveryContext deliveryContext , IDateConversionService dateConversionService )
        {
            _context = deliveryContext;
            _dateConversionService = dateConversionService;
        }

        // This method create a new Destination object 
        public void Create(Delivery createDelivery)
        {
            _context.Delivery.Add(createDelivery);
            SaveChanges();
        }
      
        // This method retrieves all data and converts the date to Persian date. 
        // We perform a projection to get all the data. However, we have a date
        // conversion operation in this process. To make the query work, we first
        // retrieve the data without performing the date conversion. 
        // Once we have our data, then we proceed with the date conversion.
        public List<DeliveryViewModel> GetAll()
        {
            var deliveries = _context.Delivery.Include(x => x.Destination).Where(x =>x.IsRemoved == false).ToList(); 
            var query = deliveries.Select(x => new DeliveryViewModel
            {
                Id = x.Id,
                DeliveryTime = x.DeliveryTime,
                PersianDeliveryTime = _dateConversionService.ToPersiandate(x.DeliveryTime),
                IsPaid = x.IsPaid,          
                Price = x.Destination.Price,
                Destination = x.Destination.DestinationName,
            });

            return query.OrderByDescending(x => x.Id).ToList();
        }

        // This method is designed to retrieve all the 'Delivery'
        // records from the database where the 'IsPaid' property
        // is false and the 'IsRemoved' property is also false. 
        // we use this query in application layer to do the Checkout 
        public List<Delivery> GetPayments()
        {
            return _context.Delivery
                .Where(x =>x.IsPaid == false && x.IsRemoved == false)
                .ToList();
        }

        // This method is utilized for obtaining the
        // details of our object for editing purposes.
        // It retrieves the current state of the object,
        // but does not directly apply any edits. 
        public EditDelivery GetEditDetailes(int id)
        {
            return _context.Delivery.Include(x =>x.Destination).Select(x => new EditDelivery
            {
                Id = x.Id,
                DeliveryTime = x.DeliveryTime,
                IsPaid = x.IsPaid,
                DestinationName = x.Destination.DestinationName,
                DestinationId = x.DestinationId
                           
            }).FirstOrDefault(x => x.Id == id);
        }

        // This method Save the Changes in databasse 
        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        // This method retrieves a Delivery object by its unique identifier (Id).
        // It searches the Delivery DbSet for the first entry that matches the provided Id.
        // If no matching entry is found, it returns null. 
        public Delivery Get(int id)
        {
            return _context.Delivery.FirstOrDefault(x => x.Id == id);
        }

        // This method, GetPaidPrice(), is responsible for calculating
        // and returning the total price of all deliveries that have
        // been paid for and are not removed from the system.
        public double GetPaidPrice()
        {
            return _context.Delivery
                .Include(x =>x.Destination)
                .Where(x =>x.IsRemoved == false && x.IsPaid == true)
                .Sum(x =>x.Destination.Price);
        }

        // This method, GetNotPaidPrice(), is responsible for calculating
        // and returning the total price of all deliveries that have not
        // been paid for and are not removed from the system.
        public double GetNotPaidPrice()
        {
            return _context.Delivery
                .Include(x =>x.Destination)
                .Where(x => x.IsRemoved == false && x.IsPaid == false)
                .Sum(x => x.Destination.Price);
        }

        // The 'Search' method filters deliveries based on a search string.
        // If the search string is a valid Persian date, it filters by date.
        // If not, it assumes the search string is a destination name and filters by that.
        // It returns a list of 'DeliveryViewModel' objects based on the filtered deliveries.
        public List<DeliveryViewModel> Search(string search)
        {
            var deliveries = _context.Delivery
                .Include(x => x.Destination)
                .Where(x => x.IsRemoved == false);

            if (!string.IsNullOrWhiteSpace(search))
            {
                DateTime? ConvertDate = _dateConversionService.toGregoriandateForSearch(search);

                if (ConvertDate != null)
                {
                    deliveries = deliveries.Where(x => x.DeliveryTime == ConvertDate);
                }
                else
                {
                    deliveries = deliveries.Where(x => x.Destination.DestinationName.Contains(search));
                }
            }

            var deliveryList = deliveries.ToList();

            var query = deliveryList.Select(x => new DeliveryViewModel
            {
                Id = x.Id,
                DeliveryTime = x.DeliveryTime,
                PersianDeliveryTime = _dateConversionService.ToPersiandate(x.DeliveryTime),
                IsPaid = x.IsPaid,
                Price = x.Destination.Price,
                Destination = x.Destination.DestinationName,
            });

            return query.ToList();
        }



    }
}
