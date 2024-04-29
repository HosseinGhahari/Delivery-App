using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery_Application_Contracts.Delivery
{
    // The IDeliveryApplication interface outlines the operations for
    // a delivery application.It includes methods for creation, editing,
    // conversions , retrieval of all destinations, and fetching details
    // for editing a destination.Each method has specific inputs and outputs.
    // also we take our dependencies from classes in this layer
    public interface IDeliveryApplication
    {
        void Create(CreateDelivery command);
        DateTime toGregoriandate(string persiandate);
        string ToPersiandate(DateTime Gregoriandate);
    }
}
