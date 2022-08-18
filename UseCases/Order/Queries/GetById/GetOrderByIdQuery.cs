using Application;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.Order.Queries.GetById
{
    public class GetOrderByIdQuery: IRequest<OrderDto>
    {

        public int Id { get; set; }
    }
}
