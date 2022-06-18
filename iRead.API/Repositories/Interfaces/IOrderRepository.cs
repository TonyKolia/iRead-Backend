using iRead.API.Models;
using iRead.API.Models.Order;

namespace iRead.API.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<string> CreateOrder(NewOrder newOrder);
        Task<OrderResponse> GetOrder(int id);
        Task<IEnumerable<OrderResponse>> GetUserOrders(int userId);
        Task<IEnumerable<OrderResponse>> GetUsersOrders(IEnumerable<int> userIds);
        Task<IEnumerable<OrderResponse>> GetUserActiveOrders(int userId);
    }
}
