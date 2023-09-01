using readerzone_api.Models;

namespace readerzone_api.Services.NotificationService
{
    public interface INotificationService
    {
        public List<Notification> GetNotifications(int pageNumber, int pageSize, out int totalNotification);
    }
}
