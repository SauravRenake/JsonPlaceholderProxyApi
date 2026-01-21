using Microsoft.AspNetCore.Mvc;
using JsonPlaceholderProxyApi.Models;
using JsonPlaceholderProxyApi.Services.Interfaces;

namespace JsonPlaceholderProxyApi.Controllers
{
    [ApiController]
    [Route("posts")]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            return Ok(await _postService.GetPostsAsync());
        }

        [HttpGet("{postId}")]
        public async Task<IActionResult> GetPost(int postId)
        {
            return Ok(await _postService.GetPostByIdAsync(postId));
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(PostDto post)
        {
            return Ok(await _postService.CreatePostAsync(post));
        }

        [HttpPut("{postId}")]
        public async Task<IActionResult> UpdatePost(int postId, PostDto post)
        {
            return Ok(await _postService.UpdatePostAsync(postId, post));
        }

        [HttpPatch("{postId}")]
        public async Task<IActionResult> PatchPost(int postId, object post)
        {
            return Ok(await _postService.PatchPostAsync(postId, post));
        }

        [HttpDelete("{postId}")]
        public async Task<IActionResult> DeletePost(int postId)
        {
            return Ok(await _postService.DeletePostAsync(postId));
        }
    }
}
