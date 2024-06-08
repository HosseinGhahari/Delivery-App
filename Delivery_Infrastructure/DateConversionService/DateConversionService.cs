using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery_Infrastructure.DateConversionService
{
    public class DateConversionService : IDateConversionService
    {
        private static PersianCalendar pc = new PersianCalendar();

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
