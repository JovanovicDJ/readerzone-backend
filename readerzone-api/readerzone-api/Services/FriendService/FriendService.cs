using Microsoft.EntityFrameworkCore;
using readerzone_api.Data;
using readerzone_api.Exceptions;
using readerzone_api.Models;
using System.Security.Claims;

namespace readerzone_api.Services.FriendService
{
    public class FriendService : IFriendService
    {
        private readonly ReaderZoneContext _readerZoneContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FriendService(ReaderZoneContext readerZoneContext, IHttpContextAccessor httpContextAccessor)
        {
            _readerZoneContext = readerZoneContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public void AddFriend(int friendId)
        {
            var result = 0;
            if (_httpContextAccessor.HttpContext != null)
            {
                result = Int32.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            }
            var customer = _readerZoneContext.Customers.Include(c => c.Friends).FirstOrDefault(customer => customer.Id == result) ?? throw new NotFoundException($"Customer with ID {result} was not found!");
            var friend = _readerZoneContext.Customers.FirstOrDefault(customer => customer.Id == friendId) ?? throw new NotFoundException($"Customer with ID {result} was not found!");
            customer.Friends.Add(friend);
            friend.Friends.Add(customer);
            _readerZoneContext.SaveChanges();

        }

        public List<Customer> GetFriends()
        {
            var result = 0;
            if (_httpContextAccessor.HttpContext != null)
            {
                result = Int32.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            }
            var customer = _readerZoneContext.Customers.Include(c => c.Friends).FirstOrDefault(customer => customer.Id == result) ?? throw new NotFoundException($"Customer with ID {result} was not found!");
            return customer.Friends.ToList();
        }

        public List<Customer> GetPossibleFrinds(string q)
        {
            var result = 0;
            if (_httpContextAccessor.HttpContext != null)
            {
                result = Int32.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            }
            var customers = _readerZoneContext.Customers
                               .Include(c => c.UserAccount)
                               .Include(c => c.Friends)
                               .Where(customer =>
                                     customer.Id != result &&
                                     (customer.Name.Contains(q) ||
                                      customer.Surname.Contains(q) ||
                                      customer.UserAccount.Username.Contains(q)))
                               .ToList();
            foreach (var customer in customers)
            {
                customer.UserAccount.PasswordHash = new byte[32];
                customer.UserAccount.PasswordSalt = new byte[32];
            }
            return customers;
        }
    }
}
