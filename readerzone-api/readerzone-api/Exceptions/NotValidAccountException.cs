namespace readerzone_api.Exceptions
{
    public class NotValidAccountException : Exception
    {
        public NotValidAccountException() : base() { }

        public NotValidAccountException(string message) : base(message) { }
    }
}
