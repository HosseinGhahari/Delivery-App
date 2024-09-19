using Delivery_Domain.DeliveryAgg;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery_Domain.AuthAgg
{
    public class User : IdentityUser
    {
        public string CustomField { get; private set; }
        public bool IsRemoved { get; private set; } 
        public ICollection<Delivery> Deliveries { get; set; } 
        public User()
        {
            Deliveries = new List<Delivery>();
            IsRemoved = false;
        }
        public void UpdateCustomField(string customField)
        {
            CustomField = customField;
        }

        public void Remove()
        {
            IsRemoved = true;
        }
    }

}
