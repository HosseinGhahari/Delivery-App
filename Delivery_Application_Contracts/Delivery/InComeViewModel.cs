using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery_Application_Contracts.Delivery
{
    // InComeViewModel represents a model for holding
    // income data related to a specific month.
    public class InComeViewModel
    {
        public string FirstDayOfMonth { get; set; }
        public string LastDayOfMonth { get; set; }
        public double InCome { get; set; }
    }
}
