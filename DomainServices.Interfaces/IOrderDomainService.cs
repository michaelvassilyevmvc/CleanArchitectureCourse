using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainServices.Interfaces
{
    public interface IOrderDomainService
    {
        decimal GetTotal(Order order, CalculateDeliveryCost deliveryCostCalculator);
    }
}
