using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using readerzone_api.Dtos;
using readerzone_api.Services.OrderService;

namespace readerzone_api.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public ActionResult AddOrder(OrderDto orderDto)
        {
            _orderService.AddOrder(orderDto);
            return Ok();
        }

        [HttpGet("{id}")]
        public ActionResult CompleteOrder(int id)
        {
            _orderService.CompleteOrder(id);
            return Ok();
        }
    }
}
