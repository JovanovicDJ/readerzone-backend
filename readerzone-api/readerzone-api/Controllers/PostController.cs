using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using readerzone_api.Dtos;
using readerzone_api.Models;
using readerzone_api.Services.PostService;

namespace readerzone_api.Controllers
{
    [Route("api/post")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [Produces("application/json")]
        [HttpGet, Authorize(Roles = "Customer")]
        public ActionResult<List<PostDto>> GetPosts(int pageNumber, int pageSize)
        {
            var posts = _postService.GetCustomerPosts(pageNumber, pageSize, out int totalPosts);
            return Ok(new { Posts = posts, TotalPosts = totalPosts });
        }

        [HttpPost("comment"), Authorize(Roles = "Customer")]
        public ActionResult<CommentDto> CommentPost(CommentPostDto commentPostDto)
        {
            var comment = _postService.CommentPost(commentPostDto.PostId, commentPostDto.Text);
            return Ok(comment);
        }

        [HttpDelete("comment/{commentId}"), Authorize(Roles = "Customer")]
        public ActionResult DeleteComment(int commentId)
        {
            _postService.DeleteComment(commentId);
            return Ok();
        }
    }
}
