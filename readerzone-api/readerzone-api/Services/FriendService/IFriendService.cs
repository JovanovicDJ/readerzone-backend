using readerzone_api.Models;

namespace readerzone_api.Services.FriendService
{
    public interface IFriendService
    {
        public void AddFriend(int friendId);
        public List<Customer> GetPossibleFrinds(string query);
        public List<Customer> GetFriends();
    }
}
