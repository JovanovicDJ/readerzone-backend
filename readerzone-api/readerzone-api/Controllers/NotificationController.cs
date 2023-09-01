using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using readerzone_api.Dtos;
using readerzone_api.Services.NotificationService;

namespace readerzone_api.Controllers
{
    [Route("api/notification")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [Produces("application/json")]
        [HttpGet, Authorize(Roles = "Customer")]
        public ActionResult<List<PostDto>> GetPosts(int pageNumber, int pageSize)
        {
            var notifications = _notificationService.GetNotifications(pageNumber, pageSize, out int totalNotification);
            return Ok(new { Notifications = notifications, TotalNotifications = totalNotification });
        }


    }
}
