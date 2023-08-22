using Microsoft.EntityFrameworkCore;
using readerzone_api.Data;
using readerzone_api.Exceptions;
using readerzone_api.Models;

namespace readerzone_api.Services.PublisherService
{
    public class PublisherService : IPublisherService
    {
        private readonly ReaderZoneContext _readerZoneContext;

        public PublisherService(ReaderZoneContext readerZoneContext)
        {
            _readerZoneContext = readerZoneContext;
        }

        public Publisher GetPublisherById(int id)
        {
            var publisher = _readerZoneContext.Publishers.Include(p => p.Address).FirstOrDefault(p => p.Id == id);
            return publisher == null ? throw new NotFoundException($"Publisher with ID {id} has not been found.") : publisher;
        }
    }
}
