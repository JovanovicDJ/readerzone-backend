using readerzone_api.Dtos;
using readerzone_api.Models;

namespace readerzone_api.Services.OrderService
{
    public interface IOrderService
    {
        public void AddOrder(OrderDto orderDto);
        public void CompleteOrder(int id);
        public List<Order> GetPendingOrders(int pageNumber, int pageSize, out int totalOrders);
    }
}
