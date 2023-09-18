using readerzone_api.Dtos;
using readerzone_api.Models;

namespace readerzone_api.Services.PublisherService
{
    public interface IPublisherService
    {
        public Publisher AddPublisher(PublisherDto publisherDto);
        public List<BookData> GetPublisherBooks(int id);
        public Publisher GetPublisherById(int id);
        public List<Publisher> GetPublishers();
    }
}
