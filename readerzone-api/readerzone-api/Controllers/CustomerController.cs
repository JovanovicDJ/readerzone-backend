using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using readerzone_api.Data;
using readerzone_api.Dtos;
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

        [HttpGet("id/{id}")]
        public ActionResult<Customer> GetCustomerById(int id)
        {
            var customer = _customerService.GetCustomerById(id);
            return Ok(customer);
        }

        [HttpGet("books/{id}"), Authorize(Roles = "Customer")]
        public ActionResult GetPurchasedBooksByCustomerId(int id)
        {
            var wantToReadBooks = _customerService.GetPurchasedBooksByCustomerId(id, BookStatus.WantToRead);
            var readingBooks = _customerService.GetPurchasedBooksByCustomerId(id, BookStatus.Reading);
            var readBooks = _customerService.GetPurchasedBooksByCustomerId(id, BookStatus.Read);
            return Ok(new { WantToRead = wantToReadBooks, Reading = readingBooks, Read = readBooks });
        }

        [HttpGet("books/data/{id}")]
        public ActionResult<List<BookData>> GetBooksDataByCustomerId(int id)
        {
            var books = _customerService.GetBooksDataByCustomerId(id);
            return Ok(books);
        }

        [HttpPatch("{purchasedBookId}"), Authorize(Roles = "Customer")]
        public ActionResult UpdatePurchasedBookStatus(int purchasedBookId, BookStatus newStatus)
        {
            _customerService.UpdatePurchasedBookStatus(purchasedBookId, newStatus);
            return Ok();
        }

        [HttpPost("review"), Authorize(Roles = "Customer")]
        public ActionResult AddReview(ReviewDto reviewDto)
        {
            _customerService.AddReview(reviewDto.Title, reviewDto.Text, reviewDto.Rating, reviewDto.PurchasedBookId);
            return Ok();
        }

        [HttpPut, Authorize(Roles = "Customer")]
        public ActionResult UpdateCustomer(UpdateCustomerDto updateCustomerDto)
        {
            _customerService.UpdateCustomer(updateCustomerDto);
            return Ok();
        }

    }
}
