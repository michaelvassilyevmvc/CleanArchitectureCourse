using System.Threading.Tasks;

namespace Application
{
    public interface IOrderService
    {
        Task<OrderDto> GetByIdAsync(int id);
    }
}
