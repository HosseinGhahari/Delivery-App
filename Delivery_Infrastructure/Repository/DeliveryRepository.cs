using Delivery_Application_Contracts.Delivery;
using Delivery_Domain.DeliveryAgg;
using Delivery_Domain.DestinationAgg;
using Delivery_Infrastructure.Context;
using Delivery_Infrastructure.DateConversionService;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
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
        public async Task CreateAsync(Delivery createDelivery)
        {
            await _context.Delivery.AddAsync(createDelivery);
            await SaveChangesAsync();
        }

        // This method is designed to retrieve all the 'Delivery'
        // records from the database where the 'IsPaid' property
        // is false and the 'IsRemoved' property is also false. 
        // we use this query in application layer to do the Checkout 
        public async Task<List<Delivery>> GetPaymentsAsync()
        {
            return await _context.Delivery
                .Where(x => !x.IsPaid && !x.IsRemoved)
                .ToListAsync();
        }

        // This method is utilized for obtaining the
        // details of our object for editing purposes.
        // It retrieves the current state of the object,
        // but does not directly apply any edits. 
        public async Task<EditDelivery> GetEditDetailsAsync(int id)
        {
            return await _context.Delivery
                .Include(x => x.Destination)
                .Where(x => x.Id == id)
                .Select(x => new EditDelivery
                {
                    Id = x.Id,
                    DeliveryTime = x.DeliveryTime,
                    IsPaid = x.IsPaid,
                    DestinationName = x.Destination.DestinationName,
                    DestinationId = x.DestinationId,
                })
                .FirstOrDefaultAsync();
        }

        // This method Save the Changes in databasse 
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        // This method retrieves a Delivery object by its unique identifier (Id).
        // It searches the Delivery DbSet for the first entry that matches the provided Id.
        // If no matching entry is found, it returns null. 
        public async Task<Delivery> GetAsync(int id)
        {
            return await _context.Delivery.FirstOrDefaultAsync(x => x.Id == id);
        }

        // This method, GetPaidPrice(), is responsible for calculating
        // and returning the total price of all deliveries that have
        // been paid for and are not removed from the system.
        public async Task<double> GetPaidPriceAsync()
        {
            return await _context.Delivery
                .Include(x => x.Destination)
                .Where(x => !x.IsRemoved && x.IsPaid)
                .SumAsync(x => x.Destination.Price);
        }

        // This method, GetNotPaidPrice(), is responsible for calculating
        // and returning the total price of all deliveries that have not
        // been paid for and are not removed from the system.
        public async Task<double> GetNotPaidPriceAsync()
        {
            return await _context.Delivery
                .Include(x => x.Destination)
                .Where(x => !x.IsRemoved && !x.IsPaid)
                .SumAsync(x => x.Destination.Price);
        }

        // The 'Search' method filters deliveries based on a search string.
        // If the search string is a valid Persian date, it filters by date.
        // If not, it assumes the search string is a destination name and filters by that.
        // It returns a list of 'DeliveryViewModel' objects based on the filtered deliveries.
        public async Task<List<DeliveryViewModel>> SearchAsync(string search , string userId)
        {
        var deliveries = _context.Delivery
            .Include(x => x.Destination)
            .Where(x => !x.IsRemoved && x.UserId == userId);

        if (!string.IsNullOrWhiteSpace(search))
        {
            DateTime? convertDate = _dateConversionService.toGregoriandateForSearch(search);

            if (convertDate != null)
            {
                deliveries = deliveries.Where(x => x.DeliveryTime == convertDate);
            }
            else
            {
                deliveries = deliveries.Where(x => x.Destination.DestinationName.Contains(search));
            }
        }

        var deliveryList = await deliveries.ToListAsync();

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

        // This method calculates monthly income from paid deliveries
        // that have not been removed.It projects delivery data to include
        // Persian date strings and prices, groups them by the first day
        // of the Persian month,and aggregates the total income for each
        // month into a list of InComeViewModel objects.
        public async Task<List<InComeViewModel>> GetInComeAsync()
        {
            var deliveries = await _context.Delivery
                .Include(x => x.Destination)
                .Where(x => !x.IsRemoved && x.IsPaid)
                .OrderByDescending(x => x.Id)
                .Select(x => new
                {
                    PersianDateString = _dateConversionService.ToPersiandate(x.DeliveryTime),
                    Price = x.Destination.Price
                })
                .ToListAsync();

            var income = deliveries
                .GroupBy(x => _dateConversionService.GetFirstDayOfPersianMonth(x.PersianDateString.Substring(0, 8)))
                .Select(g => new InComeViewModel
                {
                    FirstDayOfMonth = g.Key,
                    LastDayOfMonth = _dateConversionService.GetLastDayOfPersianMonth(g.Key),
                    InCome = g.Sum(x => x.Price)
                })
                .ToList();

            return income;
        }
    }
}
