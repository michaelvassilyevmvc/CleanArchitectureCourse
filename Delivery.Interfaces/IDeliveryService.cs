using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery.Interfaces
{
    public interface IDeliveryService
    {
        public decimal CalculateDeliveryCost(float weight);
    }
}
