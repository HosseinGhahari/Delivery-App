using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery_Application_Contracts.Delivery
{
    public interface IDeliveryApplication
    {
        void Create(CreateDelivery command);
        DateTime toGregoriandate(string persiandate);
        string ToPersiandate(DateTime Gregoriandate);
    }
}
