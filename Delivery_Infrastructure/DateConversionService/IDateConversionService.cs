using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery_Infrastructure.DateConversionService
{
    // for more organized and maintainable code
    // this interface handel the date converts 
    public interface IDateConversionService
    {
        DateTime toGregoriandate(string persiandate);
        DateTime? toGregoriandateForSearch(string persiandate);
        string ToPersiandate(DateTime Gregoriandate);
    }
}
