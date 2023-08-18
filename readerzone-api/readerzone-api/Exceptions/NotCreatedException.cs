namespace readerzone_api.Exceptions
{
    public class NotCreatedException : Exception
    {
        public NotCreatedException() : base() { }

        public NotCreatedException(string message) : base(message) { }
    }
}
