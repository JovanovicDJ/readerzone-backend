namespace readerzone_api.Exceptions
{
    public class FailedLoginException : Exception
    {
        public FailedLoginException() : base() { }

        public FailedLoginException(string message) : base(message) { }
    }
}
