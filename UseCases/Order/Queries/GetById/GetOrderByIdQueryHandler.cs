using Application;
using AutoMapper;
using DataAccess.Interefaces;
using Delivery.Interfaces;
using DomainServices.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UseCases.Order.Queries.GetById
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderDto>
    {
        private readonly IDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IOrderDomainService _orderDomainService;
        private readonly IDeliveryService _deliveryService;

        public GetOrderByIdQueryHandler(IMapper mapper, IDbContext dbContext, IOrderDomainService orderDomainService, IDeliveryService deliveryService)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
            this._orderDomainService = orderDomainService;
            this._deliveryService = deliveryService;
        }

        public async Task<OrderDto> Handle(GetOrderByIdQuery query, CancellationToken cancellationToken)
        {
            var order = await _dbContext.Orders
                .AsNoTracking()
                .Include(x => x.Items).ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x => x.Id == query.Id);

            if (order == null) throw new EntityNotFoundException();

            var dto = _mapper.Map<OrderDto>(order);
            dto.Total = _orderDomainService.GetTotal(order, _deliveryService.CalculateDeliveryCost);
            return dto;
        }
    }
}
