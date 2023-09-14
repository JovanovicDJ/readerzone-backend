using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using readerzone_api.Dtos;
using readerzone_api.Models;
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

        [HttpGet("{id}"), Authorize(Roles = "Admin, Manager")]
        public ActionResult CompleteOrder(int id)
        {
            _orderService.CompleteOrder(id);
            return Ok();
        }

        [Produces("application/json")]
        [HttpGet("pending"), Authorize(Roles = "Admin, Manager")]
        public ActionResult<List<Order>> GetPendingOrders(int pageNumber, int pageSize)
        {
            var orders = _orderService.GetPendingOrders(pageNumber, pageSize, out int totalOrders);
            return Ok(new { Orders = orders, TotalOrders = totalOrders });
        }            
    }
}
