using readerzone_api.Data;
using readerzone_api.Models;
using System.Security.Claims;

namespace readerzone_api.Services.NotificationService
{
    public class NotificationService : INotificationService
    {
        private readonly ReaderZoneContext _readerZoneContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public NotificationService(ReaderZoneContext readerZoneContext, IHttpContextAccessor httpContextAccessor)
        {
            _readerZoneContext = readerZoneContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public List<Notification> GetNotifications(int pageNumber, int pageSize, out int totalNotification)
        {
            var loggedUserId = GetLoggedUserId();
            var notifications = _readerZoneContext.Notifications
                                                  .Where(n => !n.Deleted && n.CustomerId == loggedUserId)
                                                  .ToList();
            totalNotification = notifications.Count();
            var notificationsPage = notifications.OrderByDescending(n => n.SendingTime)
                                                 .Skip((pageNumber - 1) * pageSize)
                                                 .Take(pageSize)
                                                 .ToList();
            return notificationsPage;
        }

        private int GetLoggedUserId()
        {
            var result = 0;
            if (_httpContextAccessor.HttpContext != null)
            {
                result = Int32.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            }
            return result;
        }
    }
}
