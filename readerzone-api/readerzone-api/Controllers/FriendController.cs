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
        public ActionResult<List<Customer>> GetPossibleFriends(string query)
        {
            var customers = _friendService.GetPossibleFrinds(query);
            return Ok(customers);
        }

        [HttpGet("add/{friendId}"), Authorize(Roles = "Customer")]
        public ActionResult AddFriend(int friendId)
        {
            _friendService.AddFriend(friendId);
            return Ok();
        }

        [HttpGet, Authorize(Roles = "Customer")]
        public ActionResult<List<Customer>> GetFriends()
        {
            var friends = _friendService.GetFriends();
            return Ok(friends);
        }
    }
}
