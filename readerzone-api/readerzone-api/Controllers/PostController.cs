﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using readerzone_api.Dtos;
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

        [HttpGet, Authorize(Roles = "Customer")]
        public ActionResult<List<PostDto>> GetPosts(int pageNumber, int pageSize)
        {
            var posts = _postService.GetCustomerPosts(pageNumber, pageSize);
            return Ok(posts);
        }
    }
}
