﻿using Delivery_Domain.AuthAgg;
using Delivery_Domain.DeliveryAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery_Domain.DestinationAgg
{
    // In this section, we define the necessary properties
    // for our Destination model. We also establish a constructor
    // for the model and implement methods for editing

    // As this model forms the '1' part of the database relationship,
    // it includes List of Deliveries inside the model

    public class Destination
    {
        public int Id { get; set; }
        public string DestinationName { get; set; }
        public double Price { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public List<Delivery> Deliveries { get; set; }

        public Destination() 
        {
            Deliveries = new List<Delivery>();
        }

        public Destination(string destinationName, double price , string userid)
        {
            DestinationName = destinationName;
            Price = price;
            UserId = userid;
            Deliveries = new List<Delivery>();
        }

        public void Edit(string destinationName, double price)
        {
            DestinationName = destinationName;
            Price = price;
        }

    }
}
