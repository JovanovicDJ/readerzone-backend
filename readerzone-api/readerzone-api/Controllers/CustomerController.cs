using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using readerzone_api.Data;
using readerzone_api.Models;
using readerzone_api.Services.CustomerService;
using static readerzone_api.Enums.Enums;

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

        [HttpGet("books/{id}"), Authorize(Roles = "Customer")]
        public ActionResult GetPurchasedBookByCustomerId(int id)
        {
            var wantToReadBooks = _customerService.GetPurchasedBooksByCustomerId(id, BookStatus.WantToRead);
            var readingBooks = _customerService.GetPurchasedBooksByCustomerId(id, BookStatus.Reading);
            var readBooks = _customerService.GetPurchasedBooksByCustomerId(id, BookStatus.Read);
            return Ok(new { WantToRead = wantToReadBooks, Reading = readingBooks, Read = readBooks });
        }

        [HttpPatch("{purchasedBookId}"), Authorize(Roles = "Customer")]
        public ActionResult UpdatePurchasedBookStatus(int purchasedBookId, BookStatus newStatus)
        {
            _customerService.UpdatePurchasedBookStatus(purchasedBookId, newStatus);
            return Ok();
        }
    }
}
