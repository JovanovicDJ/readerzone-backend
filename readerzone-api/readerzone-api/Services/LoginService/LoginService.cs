using Microsoft.AspNetCore.Http;
using Microsoft.SqlServer.Server;
using Microsoft.EntityFrameworkCore;
using readerzone_api.Data;
using readerzone_api.Models;
using static readerzone_api.Enums.Enums;
using readerzone_api.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;

namespace readerzone_api.Services.LoginService
{
    public class LoginService : ILoginService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ReaderZoneContext _readerZoneContext;

        public LoginService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ReaderZoneContext readerZoneContext)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _readerZoneContext = readerZoneContext;
        }

        public string Login(string email, string password)
        {
            var userAccount = _readerZoneContext.UserAccounts.Include(ua => ua.User).FirstOrDefault(ua => ua.Email == email);
            if (userAccount == null || VerifyPassword(password, userAccount.Password))
            {
                throw new FailedLoginException("Login credentials are incorrect.");
            }            
            if (userAccount.Blocked)
            {
                throw new FailedLoginException($"User with email {userAccount.Email} is blocked.");
            }            
            if (!userAccount.Active)
            {
                throw new FailedLoginException($"User with email {userAccount.Email} has not activated his account.");
            }
            return GenerateToken(userAccount);           
        }

        public Customer RegisterCustomer(Customer customer)
        {
            if (!IsEmailTaken(customer))
            {
                _readerZoneContext.Customers.Add(customer);
                _readerZoneContext.SaveChanges();
                customer.UserAccount.Password = "";
                return customer;
            }
            else
            {
                throw new NotCreatedException($"Customer with {customer.UserAccount.Email} email already exists.");
            }            
        }

        public Employee RegisterEmployee(Employee employee)
        {
            if (!IsEmailTaken(employee))
            {
                _readerZoneContext.Employees.Add(employee);
                _readerZoneContext.SaveChanges();
                employee.UserAccount.Password = "";
                return employee;
            }
            else
            {
                throw new NotCreatedException($"Employee with {employee.UserAccount.Email} email already exists.");
            }                       
        }

        private bool IsEmailTaken(User user)
        {
            var c = _readerZoneContext.Users.Include(c => c.UserAccount).FirstOrDefault(c => c.UserAccount.Email == user.UserAccount.Email);
            if (c != null)
            {
                return true;
            }
            return false;
        }

        private static bool VerifyPassword(string password, string correctPassword)
        {
            var sha = SHA256.Create();
            var asByteArray = Encoding.Default.GetBytes(password);
            var p = Convert.ToBase64String(sha.ComputeHash(asByteArray));
            return correctPassword.Equals(p);            
        }

        private string GenerateToken(UserAccount userAccount)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userAccount.Id.ToString()),
                new Claim(ClaimTypes.Name, userAccount.Username),
                new Claim(ClaimTypes.Email, userAccount.Email),               
                new Claim(ClaimTypes.Role, userAccount.Role.ToString())
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Key").Value));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
        
    }
}
