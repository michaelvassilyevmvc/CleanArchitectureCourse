using AutoMapper;
using DataAccess.Interefaces;
using Email.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using WebApp.Interfaces;


namespace UserCases.Order.Commands.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, int>
    {
        private readonly IMapper _mapper;
        private readonly IDbContext _dbContext;
        private readonly IBackgroundJobService _backgroundJobService;
        private readonly ICurrentUserService _currentUserService;

        public CreateOrderCommandHandler
        (
            IMapper mapper, 
            IDbContext dbContext, 
            IBackgroundJobService backgroundJobService, 
            ICurrentUserService currentUserService
        )
        {
            this._mapper = mapper;
            this._dbContext = dbContext;
            this._backgroundJobService = backgroundJobService;
            this._currentUserService = currentUserService;
        }

        public async Task<int> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            var order = _mapper.Map<Domain.Entities.Order>(command.Dto);
            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();

            _backgroundJobService.Schedule<IEmailService>(emailService => emailService.SendAsync(_currentUserService.Email, "Order created", $"Order {order.Id} created"));

            return order.Id;
        }
    }
}
