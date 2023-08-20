using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using readerzone_api.Dtos;
using readerzone_api.Models;
using readerzone_api.Services.LoginService;

namespace readerzone_api.Controllers
{
    [Route("api")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [Produces("application/json")]
        [HttpPost("login")]
        public ActionResult<string> Login(LoginDto loginDto)
        {
            string token = _loginService.Login(loginDto.Email, loginDto.Password);
            return Ok(token);
        }

        [HttpPost("register/customer")]
        public ActionResult<Customer> RegisterCustomer(CustomerDto customerDto)
        {
            var customer = _loginService.RegisterCustomer(new Customer(customerDto));
            return Ok(customer);
        }

        [HttpPost("register/employee")]
        public ActionResult<Employee> RegisterEmployee(EmployeeDto employeeDto)
        {
            var employee = _loginService.RegisterEmployee(new Employee(employeeDto));
            return Ok(employee);
        }

        [Produces("application/json")]
        [HttpGet("activate/{id}")]
        public ActionResult ActivateAccount(int id)
        {
            var status = _loginService.ActivateAccount(id);
            var angularLoginUrl = "http://localhost:4200/login/" + status;
            return Redirect(angularLoginUrl);
        }

        [HttpGet("forgot/password/{email}")]
        public ActionResult ForgottenPassword(string email)
        {
            _loginService.ForgottenPassword(email);
            return Ok();
        }

        [HttpPost("reset/password")]
        public ActionResult ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            _loginService.ResetPassword(resetPasswordDto.Password, resetPasswordDto.Token);
            return Ok();
        }
    }
}
