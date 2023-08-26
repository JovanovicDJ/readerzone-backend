using readerzone_api.Dtos;

namespace readerzone_api.Services.OrderService
{
    public interface IOrderService
    {
        public void AddOrder(OrderDto orderDto);

        public void CompleteOrder(int id);
    }
}
