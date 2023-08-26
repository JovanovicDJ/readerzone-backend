using readerzone_api.Models;

namespace readerzone_api.Services.PostService
{
    public interface IPostService
    {
        public void GeneratePurchasedBookPost(string email, ICollection<Book> books);
    }
}
