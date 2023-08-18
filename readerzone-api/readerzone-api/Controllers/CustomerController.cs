using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using readerzone_api.Data;
using readerzone_api.Models;
using readerzone_api.Services.CustomerService;

namespace readerzone_api.Controllers
{
    [Route("api/customer")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet("{email}")]
        public ActionResult<Customer> GetCustomerByEmail(string email)
        {
            var customer = _customerService.GetCustomerByEmail(email);
            return Ok(customer);
        }
    }
}
