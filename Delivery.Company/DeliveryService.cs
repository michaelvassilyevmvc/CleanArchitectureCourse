using Delivery.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery.Company
{
    public class DeliveryService : IDeliveryService
    {
        public decimal CalculateDeliveryCost(float weight)
        {
            return (decimal)weight * 10;
        }

        public Task<bool> IsDeliveryAsync(int orderId)
        {
            return Task.FromResult(true);
        }
    }
}
