using Domain.Entities;
using DomainServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainServices.Implementation
{
    public class OrderDomainService : IOrderDomainService
    {
        public decimal GetTotal(Order order, CalculateDeliveryCost deliveryCostCalculator)
        {
            var totalWeight = order.Items.Sum(x => x.Product.Weight);
            var deliveryCost = deliveryCostCalculator(totalWeight);
            var totalPrice = order.Items.Sum(x => x.Product.Price);
            return totalPrice + deliveryCost;
        }
    }
}
