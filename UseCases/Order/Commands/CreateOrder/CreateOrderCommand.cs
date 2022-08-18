using Application;
using MediatR;

namespace UserCases.Order.Commands.CreateOrder
{
    public class CreateOrderCommand: IRequest<int>
    {
        public CreateOrderDto Dto { get; set; }
    }
}
