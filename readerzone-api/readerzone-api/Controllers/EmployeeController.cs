using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using readerzone_api.Models;
using readerzone_api.Services.EmployeeService;

namespace readerzone_api.Controllers
{
    [Route("api/employee")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet("{email}")]
        public ActionResult<Employee> GetEmployeeByEmail(string email)
        {
            var employee = _employeeService.GetEmployeeByEmail(email);
            return Ok(employee);
        }
    }
}
