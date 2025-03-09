using Delivery_Application_Contracts.Delivery;
using Delivery_Domain.DeliveryAgg;
using Delivery_Domain.DestinationAgg;
using Delivery_Infrastructure.Context;
using Delivery_Infrastructure.DateConversionService;
using Microsoft.AspNetCore.Mvc;
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
                    OptionalPrice = x.OptionalPrice
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

        // This method, GetPaidPriceAsync(), calculates and returns the total price 
        // of all deliveries that have been paid and are not marked as removed.
        // It sums the OptionalPrice if available; otherwise, it uses the price 
        // from the associated destination for each delivery.
        public async Task<double> GetPaidPriceAsync()
        {
            return await _context.Delivery
                .Include(x => x.Destination)
                .Where(x => !x.IsRemoved && x.IsPaid)
                .SumAsync(x => x.OptionalPrice.HasValue ? x.OptionalPrice.Value : x.Destination.Price);
        }

        // This method, GetNotPaidPriceAsync(), calculates and returns the total price 
        // of all deliveries that have not been paid and are not marked as removed.
        // It sums the OptionalPrice if available; otherwise, it uses the price 
        // from the associated destination for each delivery.
        public async Task<double> GetNotPaidPriceAsync()
        {
            return await _context.Delivery
                .Include(x => x.Destination)
                .Where(x => !x.IsRemoved && !x.IsPaid)
                .SumAsync(x => x.OptionalPrice.HasValue ? x.OptionalPrice.Value : x.Destination.Price);
        }

        // Retrieves a list of deliveries for a specific user, excluding
        // removed deliveries.The results are ordered by the most recent deliveries
        // first.Each delivery is mapped to a 'DeliveryViewModel', converting the
        // delivery date to Persian format.
        public async Task<List<DeliveryViewModel>> GetDeliveries(string userId)
        {
            var deliveries = await _context.Delivery
                  .Include(x => x.Destination)
                  .Where(x => !x.IsRemoved && x.UserId == userId)
                  .OrderByDescending(d => d.Id)
                  .ToListAsync();

            return deliveries.Select(x => new DeliveryViewModel
            {
                Id = x.Id,
                DeliveryTime = x.DeliveryTime,
                PersianDeliveryTime = _dateConversionService.ToPersiandate(x.DeliveryTime),
                IsPaid = x.IsPaid,
                Price = x.Destination.Price,
                Destination = x.Destination.DestinationName,
                OptionalPrice = x.OptionalPrice

            }).ToList();
        }

        // This method calculates the total income from paid, non-removed deliveries.
        // It converts delivery dates to Persian, groups by the first day of the Persian month,
        // and aggregates the total income for each month into a list of InComeViewModel objects.
        public async Task<List<InComeViewModel>> GetInComeAsync()
        {
            var deliveries = await _context.Delivery
                .Include(x => x.Destination)
                .Where(x => !x.IsRemoved && x.IsPaid)
                .OrderByDescending(x => x.Id)
                .Select(x => new
                {
                    PersianDateString = _dateConversionService.ToPersiandate(x.DeliveryTime),
                    Price = x.OptionalPrice.HasValue ? x.OptionalPrice.Value : x.Destination.Price
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
