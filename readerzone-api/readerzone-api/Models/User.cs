namespace readerzone_api.Models
{
    public abstract class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public DateTime Dob { get; set; }
        public UserAccount UserAccount { get; set; } = null!;
        public Address Address { get; set; } = null!;
        public string PhoneNumber { get; set; } = string.Empty;

        public User()
        {
            
        }

        public User(string name, string surname, DateTime dob, string phoneNumber)
        {
            Name = name;
            Surname = surname;
            Dob = dob;
            PhoneNumber = phoneNumber;
        }        
    }
}
