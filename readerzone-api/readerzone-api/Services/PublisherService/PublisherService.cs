using Microsoft.EntityFrameworkCore;
using readerzone_api.Data;
using readerzone_api.Dtos;
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

        public List<Publisher> GetPublishers()
        {
            var publishers = _readerZoneContext.Publishers.ToList();
            return publishers == null ? throw new NotFoundException("Publishers not found.") : publishers; 
        }

        public Publisher AddPublisher(PublisherDto publisherDto)
        {
            var publisher = _readerZoneContext.Publishers.FirstOrDefault(p => p.Name.Equals(publisherDto.Name));
            if (publisher != null)
            {
                throw new NotCreatedException($"Publisher {publisherDto.Name} already exists.");
            }
            publisher = new Publisher()
            {
                Name = publisherDto.Name,
                Established = DateTime.ParseExact(publisherDto.Established, "dd.MM.yyyy.", null, System.Globalization.DateTimeStyles.None),
                Address = new Address(publisherDto.Street, publisherDto.Number, publisherDto.City, publisherDto.PostalCode, publisherDto.Country)
            };
            _readerZoneContext.Publishers.Add(publisher);
            _readerZoneContext.SaveChanges();
            return publisher;
        }
    }
}
