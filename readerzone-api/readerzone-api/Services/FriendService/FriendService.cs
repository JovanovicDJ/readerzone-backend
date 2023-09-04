using Microsoft.EntityFrameworkCore;
using readerzone_api.Data;
using readerzone_api.Exceptions;
using readerzone_api.Models;
using System.Security.Claims;
using static readerzone_api.Enums.Enums;

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

        public void SendFriendRequest(int customerId)
        {
            var loggedUserId = GetLoggedUserId();
            var existingNotification = _readerZoneContext.Notifications.FirstOrDefault(n => n.CustomerId == customerId &&
                                                            n.FromCustomerId == loggedUserId &&
                                                            n.NotificationType == NotificationType.FriendRequest &&
                                                            !n.Deleted);
            if (existingNotification != null)
            {
                throw new NotCreatedException("Friendship request already sent.");
            }
            var loggedUser = _readerZoneContext.Customers.Include(c => c.UserAccount).FirstOrDefault(c => c.Id == loggedUserId) ?? throw new NotFoundException($"Customer with ID {loggedUserId} was not found.");
            var notification = new Notification()
            {
                CustomerId = customerId,
                FromCustomerId = loggedUser.Id,
                FromCustomerName = loggedUser.Name,
                FromCustomerSurname = loggedUser.Surname,
                FromCustomerUsername = loggedUser.UserAccount.Username,
                Text = $"{loggedUser.Name} {loggedUser.Surname} wants to be your friend.",
                SendingTime = DateTime.Now,
                NotificationType = NotificationType.FriendRequest,
                Deleted = false
            };
            _readerZoneContext.Notifications.Add(notification);
            _readerZoneContext.SaveChanges();
                                                            
        }

        public void AddFriend(int friendId)
        {
            var loggedUserId = GetLoggedUserId();
            var customer = _readerZoneContext.Customers.Include(c => c.Friends).Include(c => c.UserAccount).FirstOrDefault(customer => customer.Id == loggedUserId) ?? throw new NotFoundException($"Customer with ID {loggedUserId} was not found!");
            var friend = _readerZoneContext.Customers.Include(c => c.Friends).FirstOrDefault(customer => customer.Id == friendId) ?? throw new NotFoundException($"Customer with ID {loggedUserId} was not found!");
            customer.Friends.Add(friend);
            friend.Friends.Add(customer);
            _readerZoneContext.SaveChanges();
            SendFriendshipAcceptedNotification(customer, friend);
            DeleteFriendshipRequest(friendId, loggedUserId);
        }
        
        public void DeleteFriend(int friendId)
        {
            var loggedUserId = GetLoggedUserId();
            var customer = _readerZoneContext.Customers.Include(c => c.Friends).Include(c => c.UserAccount).FirstOrDefault(customer => customer.Id == loggedUserId) ?? throw new NotFoundException($"Customer with ID {loggedUserId} was not found!");
            var friend = _readerZoneContext.Customers.Include(c => c.Friends).FirstOrDefault(customer => customer.Id == friendId) ?? throw new NotFoundException($"Customer with ID {loggedUserId} was not found!");
            customer.Friends.Remove(friend);
            friend.Friends.Remove(customer);
            _readerZoneContext.SaveChanges();
        }

        public void RejectFriendship(int notificationId)
        {
            var notification = _readerZoneContext.Notifications.FirstOrDefault(n => n.Id == notificationId && n.NotificationType == NotificationType.FriendRequest);
            if (notification == null)
            {
                throw new NotFoundException($"Notification with ID {notificationId} was not found");
            }
            notification.Deleted = true;
            _readerZoneContext.SaveChanges();
        }

        public List<Customer> GetFriends()
        {
            var loggedUserId = GetLoggedUserId();
            var customer = _readerZoneContext.Customers.Include(c => c.Friends).ThenInclude(f => f.UserAccount).FirstOrDefault(customer => customer.Id == loggedUserId) ?? throw new NotFoundException($"Customer with ID {loggedUserId} was not found!");

            foreach (var friend in customer.Friends.ToList())
            {
                friend.UserAccount.PasswordHash = new byte[32];
                friend.UserAccount.PasswordSalt = new byte[32];
            }

            return customer.Friends.ToList();
        }

        public List<Customer> GetFriendsForCustomer(int customerId)
        {            
            var customer = _readerZoneContext.Customers.Include(c => c.Friends).ThenInclude(f => f.UserAccount).FirstOrDefault(customer => customer.Id == customerId) ?? throw new NotFoundException($"Customer with ID {customerId} was not found!");

            foreach (var friend in customer.Friends.ToList())
            {
                friend.UserAccount.PasswordHash = new byte[32];
                friend.UserAccount.PasswordSalt = new byte[32];
            }

            return customer.Friends.ToList();
        }

        public List<Customer> GetPossibleFrinds(string q)
        {
            var loggedUserId = GetLoggedUserId();
            var customers = _readerZoneContext.Customers
                               .Include(c => c.UserAccount)
                               .Include(c => c.Friends)
                               .Where(customer =>
                                     customer.Id != loggedUserId &&
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

        private int GetLoggedUserId()
        {
            var result = 0;
            if (_httpContextAccessor.HttpContext != null)
            {
                result = Int32.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            }
            return result;
        }

        private void SendFriendshipAcceptedNotification(Customer customer, Customer friend)
        {
            var notification = new Notification()
            {
                CustomerId = friend.Id,
                FromCustomerId = customer.Id,
                FromCustomerName = customer.Name,
                FromCustomerSurname = customer.Surname,
                FromCustomerUsername = customer.UserAccount.Username,
                Text = $"{customer.Name} {customer.Surname} has accepted your friendship request.",
                SendingTime = DateTime.Now,
                NotificationType = NotificationType.FriendshipAccepted,
                Deleted = false
            };
            _readerZoneContext.Notifications.Add(notification);
            _readerZoneContext.SaveChanges();
        }

        private void DeleteFriendshipRequest(int friendId, int loggedUserId)
        {
            var notification = _readerZoneContext.Notifications.FirstOrDefault(n => n.NotificationType == NotificationType.FriendRequest &&
                                                                                    n.FromCustomerId == friendId &&
                                                                                    n.CustomerId == loggedUserId);
            if (notification == null)
            {
                throw new NotFoundException($"Notification was not found");
            }
            notification.Deleted = true;
            _readerZoneContext.SaveChanges();
        }

    }
}
