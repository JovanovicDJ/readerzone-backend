using Microsoft.EntityFrameworkCore;
using readerzone_api.Data;
using readerzone_api.Exceptions;
using readerzone_api.Models;
using static readerzone_api.Enums.Enums;

namespace readerzone_api.Services.CustomerService
{
    public class CustomerService : ICustomerService
    {
        private readonly ReaderZoneContext _readerZoneContext;

        public CustomerService(ReaderZoneContext readerZoneContext)
        {
            _readerZoneContext = readerZoneContext;
        }
        public Customer GetCustomerByEmail(string email)
        {
            var userAccount = _readerZoneContext.UserAccounts.Include(ua => ua.User)
                                                             .ThenInclude(u => u.Address)                                                             
                                                             .FirstOrDefault(ua => ua.Email == email);
            if (userAccount != null && userAccount.Role.Equals(Role.Customer))
            {
                userAccount.PasswordHash = new byte[32];
                userAccount.PasswordSalt = new byte[32];
                return (Customer)userAccount.User;
            }
            else
            {
                throw new NotFoundException($"User with email {email} does not exist.");
            }
        }
    }
}
