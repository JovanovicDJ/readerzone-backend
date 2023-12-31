﻿using readerzone_api.Models;

namespace readerzone_api.Services.FriendService
{
    public interface IFriendService
    {
        public void AddFriend(int friendId);
        public List<Customer> FriendSearch(string query);
        public List<Customer> GetFriends();
        public List<Customer> GetFriendsForCustomer(int customerId);
        public void SendFriendRequest(int customerId);
        public void RejectFriendship(int notificationId);
        public void DeleteFriend(int friendId);
    }
}
