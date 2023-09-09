using readerzone_api.Models;

namespace readerzone_api.Services.EmployeeService
{
    public interface IEmployeeService
    {
        public Employee GetEmployeeByEmail(string email);
    }
}
