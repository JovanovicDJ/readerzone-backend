using Microsoft.EntityFrameworkCore;
using readerzone_api.Data;
using readerzone_api.Exceptions;
using readerzone_api.Models;
using static readerzone_api.Enums.Enums;

namespace readerzone_api.Services.EmployeeService
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ReaderZoneContext _readerZoneContext;        

        public EmployeeService(ReaderZoneContext readerZoneContext)
        {
            _readerZoneContext = readerZoneContext;            
        }

        public Employee GetEmployeeByEmail(string email)
        {
            var userAccount = _readerZoneContext.UserAccounts.Include(ua => ua.User)
                                                             .ThenInclude(u => u.Address)
                                                             .FirstOrDefault(ua => ua.Email == email);
            if (userAccount != null && (userAccount.Role.Equals(Role.Manager) || userAccount.Role.Equals(Role.Admin)))
            {
                userAccount.PasswordHash = new byte[32];
                userAccount.PasswordSalt = new byte[32];
                return (Employee)userAccount.User;
            }
            else
            {
                throw new NotFoundException($"User with email {email} does not exist.");
            }
        }
    }
}
