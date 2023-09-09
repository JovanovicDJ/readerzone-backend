using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using readerzone_api.Dtos;
using readerzone_api.Models;
using readerzone_api.Services.PublisherService;

namespace readerzone_api.Controllers
{
    [Route("api/publisher")]
    [ApiController]
    public class PublisherController : ControllerBase
    {
        private readonly IPublisherService _publisherService;

        public PublisherController(IPublisherService publisherService)
        {
            _publisherService = publisherService;
        }

        [HttpGet, Authorize(Roles = "Admin, Manager")]
        public ActionResult<List<Publisher>> GetPublishers()
        {
            var publishers = _publisherService.GetPublishers();
            return Ok(publishers);
        }

        [HttpPost, Authorize(Roles = "Admin, Manager")]
        public ActionResult<Publisher> AddPublisher(PublisherDto publisherDto)
        {
            var publisher = _publisherService.AddPublisher(publisherDto);
            return Ok(publisher);
        }
    }
}
