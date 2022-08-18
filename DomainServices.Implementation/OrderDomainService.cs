﻿using Domain.Entities;
using DomainServices.Interfaces;
using System.Linq;

namespace DomainServices.Implementation
{
    public class OrderDomainService : IOrderDomainService
    {

        public decimal GetTotal(Order order, CalculateDeliveryCost deliveryCostCalculator)
        {

            decimal totalPrice =  order.Items.Sum(x => x.Quantity * x.Product.Price);
            decimal deliveryCost = 0;

            if(totalPrice < 1000)
            {
                var totalWeight = order.Items.Sum(x => x.Product.Weight);
                deliveryCost = deliveryCostCalculator(totalWeight);
            }

            return totalPrice + deliveryCost;
        }
    }
}