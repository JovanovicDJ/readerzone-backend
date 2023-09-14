using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using readerzone_api.Models;
using readerzone_api.Services.FriendService;

namespace readerzone_api.Controllers
{
    [Route("api/friend")]
    [ApiController]
    public class FriendController : ControllerBase
    {
        private readonly IFriendService _friendService;

        public FriendController(IFriendService friendService)
        {
            _friendService = friendService;
        }

        [HttpGet("search"), Authorize(Roles = "Customer")]
        public ActionResult<List<Customer>> FriendSearch(string query)
        {
            var customers = _friendService.FriendSearch(query);
            return Ok(customers);
        }

        [HttpGet("request/{customerId}"), Authorize(Roles = "Customer")]
        public ActionResult SendFriendRequest(int customerId)
        {
            _friendService.SendFriendRequest(customerId);
            return Ok();
        }

        [HttpGet("add/{friendId}"), Authorize(Roles = "Customer")]
        public ActionResult AddFriend(int friendId)
        {
            _friendService.AddFriend(friendId);
            return Ok();
        }

        [HttpGet("reject/{notificationId}"), Authorize(Roles = "Customer")]
        public ActionResult RejectFriendship(int notificationId)
        {
            _friendService.RejectFriendship(notificationId);
            return Ok();
        }

        [HttpGet, Authorize(Roles = "Customer")]
        public ActionResult<List<Customer>> GetFriends()
        {
            var friends = _friendService.GetFriends();
            return Ok(friends);
        }

        [HttpGet("{customerId}"), Authorize(Roles = "Customer")]
        public ActionResult<List<Customer>> GetFriendsForCustomer(int customerId)
        {
            var friends = _friendService.GetFriendsForCustomer(customerId);
            return Ok(friends);
        }

        [HttpDelete("{friendId}"), Authorize(Roles = "Customer")]
        public ActionResult DeleteFriend(int friendId)
        {
            _friendService.DeleteFriend(friendId);
            return Ok();
        }
    }
}
