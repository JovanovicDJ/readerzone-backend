namespace readerzone_api.Enums
{
    public class Enums
    {
        public enum Role
        {
            Customer,
            Manager,
            Admin
        }

        public enum BookStatus
        {
            WantToRead,
            Reading,
            Read
        }

        public enum Tier
        {
            Bronze,
            Silver,
            Gold, 
            Platinum
        }

        public enum Status
        {
            Unactive,
            Active
        }
    }
}
