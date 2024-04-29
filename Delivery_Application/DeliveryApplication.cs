using Delivery_Application_Contracts.Delivery;
using Delivery_Domain.DeliveryAgg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Delivery_Application
{
    // Here, we inject our interface from the Infrastructure layer
    // and inherit from 'IDeliveryApplication' to perform the primary operations.
    // also provides a clear and concise explanation of how the interface from
    // the Infrastructure layer is being used and the role of ‘IDeliveryApplication’

    public class DeliveryApplication : IDeliveryApplication
    {
        private static PersianCalendar pc = new PersianCalendar();

        private readonly IDeliveryRepository _deliveryRepository;
        public DeliveryApplication(IDeliveryRepository deliveryRepository)
        {
            _deliveryRepository = deliveryRepository;
        }


        // Creates a new delivery with the given command,
        // adds it to the repository, and saves the changes
        public void Create(CreateDelivery command)
        {
            var delivery = new Delivery(command.IsPaid,command.DestinationId , command.DeliveryTime);
            _deliveryRepository.Create(delivery);
            _deliveryRepository.SaveChanges();
        }


        // Converts a Persian date string to a Gregorian date
        public DateTime toGregoriandate(string persianDate)
        {
            var dateParts = persianDate.Split('/');
            if (dateParts.Length != 3)
            {
                throw new FormatException("Invalid date format. Expected format: yyyy/MM/dd");
            }

            int year = Convert.ToInt32(dateParts[0]);
            int month = Convert.ToInt32(dateParts[1]);
            int day = Convert.ToInt32(dateParts[2]);

            DateTime gregorianDate = pc.ToDateTime(year, month, day, 0, 0, 0, 0);
            return gregorianDate;
        }


        // Converts a Gregorian date to a Persian date string
        public string ToPersiandate(DateTime Gregoriandate)
        {
            PersianCalendar pc = new PersianCalendar();
            string persianDate = string.Format("{0}/{1}/{2}",
            pc.GetYear(Gregoriandate), pc.GetMonth(Gregoriandate), pc.GetDayOfMonth(Gregoriandate));
            return persianDate;
        }
    }
}
