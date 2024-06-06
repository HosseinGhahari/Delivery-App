using Delivery_Application_Contracts.Delivery;
using Delivery_Domain.DeliveryAgg;
using Delivery_Domain.DestinationAgg;
using Delivery_Infrastructure.Context;
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
        private static PersianCalendar pc = new PersianCalendar();

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
                PersianDeliveryTime = ToPersiandate(x.DeliveryTime),
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

        // Converts a Persian date string to a Gregorian date
        public DateTime toGregoriandate(string persianDate)
        {
            var dateParts = persianDate.Split('/');

            int year = 1;
            int month = 1;
            int day = 1;

            if (dateParts.Length > 0)
            {
                year = Convert.ToInt32(dateParts[0]);
            }

            if (dateParts.Length > 1)
            {
                month = Convert.ToInt32(dateParts[1]);
            }

            if (dateParts.Length > 2)
            {
                day = Convert.ToInt32(dateParts[2]);
            }

            DateTime gregorianDate = pc.ToDateTime(year, month, day, 0, 0, 0, 0);
            return gregorianDate;
        }

        // This method converts a Persian date string to a Gregorian date.
        // Unlike typical conversion methods, this one is designed to be
        // error-tolerant. If the input format is incorrect, it won't throw
        // an error. This makes it particularly useful for search operations
        // where the input format may vary.
        public DateTime? toGregoriandateForSearch(string persianDate)
        {
            var dateParts = persianDate.Split('/');

            if (dateParts.Length != 3)
                return null;

            if (!int.TryParse(dateParts[0], out int year))
                return null;

            if (!int.TryParse(dateParts[1], out int month))
                return null;

            if (!int.TryParse(dateParts[2], out int day))
                return null;

            try
            {
                DateTime gregorianDate = pc.ToDateTime(year, month, day, 0, 0, 0, 0);
                return gregorianDate;
            }
            catch
            {
                return null;
            }
        }

        // Converts a Gregorian date to a Persian date string
        public string ToPersiandate(DateTime Gregoriandate)
        {
            PersianCalendar pc = new PersianCalendar();
            string persianDate = string.Format("{0}/{1}/{2}",
            pc.GetYear(Gregoriandate), pc.GetMonth(Gregoriandate), pc.GetDayOfMonth(Gregoriandate));
            return persianDate;
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
                DateTime? ConvertDate = toGregoriandateForSearch(search);

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
                PersianDeliveryTime = ToPersiandate(x.DeliveryTime),
                IsPaid = x.IsPaid,
                Price = x.Destination.Price,
                Destination = x.Destination.DestinationName,
            });

            return query.ToList();
        }



    }
}
