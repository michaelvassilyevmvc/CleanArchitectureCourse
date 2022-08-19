using Application;
using AutoMapper;
using DataAccess.Interefaces;
using Delivery.Interfaces;
using DomainServices.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace UseCases.Order.Queries.GetById
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderDto>
    {
        private readonly IDbContext _dbContext;
        private readonly IDeliveryService _deliveryService;
        private readonly IOrderDomainService _orderDomainService;
        private readonly IMapper _mapper;

        public GetOrderByIdQueryHandler(IMapper mapper, IDbContext dbContext, IDeliveryService deliveryService, IOrderDomainService orderDomainService)
        {
            this._dbContext = dbContext;
            _deliveryService = deliveryService;
            _orderDomainService = orderDomainService;
            this._mapper = mapper;
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