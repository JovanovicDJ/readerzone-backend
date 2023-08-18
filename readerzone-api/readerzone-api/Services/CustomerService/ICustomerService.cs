using readerzone_api.Models;

namespace readerzone_api.Services.CustomerService
{
    public interface ICustomerService
    {
        public Customer GetCustomerByEmail(string email);
    }
}
