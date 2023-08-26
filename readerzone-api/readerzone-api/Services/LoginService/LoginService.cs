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
using readerzone_api.Services.EmailService;
using Microsoft.AspNetCore.Identity;

namespace readerzone_api.Services.LoginService
{
    public class LoginService : ILoginService
    {
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;        
        private readonly ReaderZoneContext _readerZoneContext;

        public LoginService(IConfiguration configuration, IEmailService emailService, ReaderZoneContext readerZoneContext)
        {
            _configuration = configuration;
            _emailService = emailService;            
            _readerZoneContext = readerZoneContext;
        }

        public string Login(string email, string password)
        {
            var userAccount = _readerZoneContext.UserAccounts.Include(ua => ua.User).FirstOrDefault(ua => ua.Email == email);
            if (userAccount == null || !VerifyPasswordHash(password, userAccount.PasswordHash, userAccount.PasswordSalt))
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

        public Customer RegisterCustomer(Customer customer, string password)
        {
            if (!IsEmailTaken(customer))
            {                
                CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
                customer.UserAccount.PasswordSalt = passwordSalt;
                customer.UserAccount.PasswordHash = passwordHash;
                _readerZoneContext.Customers.Add(customer);
                _readerZoneContext.SaveChanges();
                _emailService.SendActivationEmail(customer.Name, customer.UserAccount.Email, customer.UserAccount.Id);                
                customer.UserAccount.PasswordHash = new byte[32];
                customer.UserAccount.PasswordSalt = new byte[32];
                return customer;
            }
            else
            {
                throw new NotCreatedException($"Customer with {customer.UserAccount.Email} email already exists.");
            }
        }

        public string ActivateAccount(int id)
        {
            var userAccount = _readerZoneContext.UserAccounts.FirstOrDefault(ua => ua.Id == id);
            if (userAccount == null) return "invalidactivation";
            else if (userAccount.Active) return "alreadyactive";
            else
            {
                userAccount.Active = true;
                _readerZoneContext.SaveChanges();
                return "success";
            }
        }

        public Employee RegisterEmployee(Employee employee, string password)
        {
            if (!IsEmailTaken(employee))
            {
                CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
                employee.UserAccount.PasswordSalt = passwordSalt;
                employee.UserAccount.PasswordHash = passwordHash;
                _readerZoneContext.Employees.Add(employee);
                _readerZoneContext.SaveChanges();                
                employee.UserAccount.PasswordHash = new byte[32];
                employee.UserAccount.PasswordSalt = new byte[32];
                return employee;
            }
            else
            {
                throw new NotCreatedException($"Employee with {employee.UserAccount.Email} email already exists.");
            }
        }

        public void ForgottenPassword(string email)
        {
            var userAccount = _readerZoneContext.UserAccounts.FirstOrDefault(ua => ua.Email == email);
            if (userAccount == null || userAccount.Blocked || !userAccount.Active)
            {
                throw new NotValidAccountException($"User account with email {email} is not valid.");
            }
            long token = DateTimeOffset.UtcNow.AddMinutes(15).ToUnixTimeMilliseconds();
            _emailService.SendForgottenPasswordEmail(email, userAccount.Id, token);

        }

        public void ResetPassword(string password, string token)
        {
            string[] tokens = token.Split('-');
            var userAccount = _readerZoneContext.UserAccounts.FirstOrDefault(ua => ua.Id == Int32.Parse(tokens[0]));
            if (userAccount == null)
            {
                throw new NotFoundException($"User with ID {tokens[0]} is not found.");
            }
            if (long.TryParse(tokens[1], out long tokenTimestamp))
            {
                DateTimeOffset tokenTime = DateTimeOffset.FromUnixTimeMilliseconds(tokenTimestamp);
                if (tokenTime > DateTimeOffset.UtcNow)
                {
                    CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
                    userAccount.PasswordSalt = passwordSalt;
                    userAccount.PasswordHash = passwordHash;
                    _readerZoneContext.SaveChanges();
                }
            }
            else
            {
                throw new NotFoundException("Reset password token has expired.");
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

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
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
