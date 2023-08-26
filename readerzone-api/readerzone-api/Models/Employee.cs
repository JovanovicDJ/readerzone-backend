using readerzone_api.Dtos;
using System.Text.Json.Serialization;
using static readerzone_api.Enums.Enums;

namespace readerzone_api.Models
{
    public class Employee : User
    {        
        public DateTime HireDate { get; set; }
        [JsonIgnore]
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public Employee(string name, string surname, DateTime dob, string phoneNumber) : base(name, surname, dob, phoneNumber)
        {
        }

        public Employee(EmployeeDto employeeDto)
        {
            DateTime dob = DateTime.ParseExact(employeeDto.Dob, "dd.MM.yyyy.", null, System.Globalization.DateTimeStyles.None);
            Name = employeeDto.Name;
            Surname = employeeDto.Surname;
            Dob = dob;
            PhoneNumber = employeeDto.PhoneNumber;
            HireDate = DateTime.Now;
            UserAccount = new UserAccount(employeeDto.Username, employeeDto.Email, Enum.Parse<Role>(employeeDto.Role), false, false);
            Address = new Address(employeeDto.Street, employeeDto.Number, employeeDto.City, employeeDto.PostalCode, employeeDto.Country);
        }
    }
}
