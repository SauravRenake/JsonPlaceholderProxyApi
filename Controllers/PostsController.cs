using Microsoft.AspNetCore.Mvc;
using JsonPlaceholderProxyApi.Models;
using JsonPlaceholderProxyApi.Services.Interfaces;
using Serilog;

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
            Log.Information("GET /posts called");

            var posts = await _postService.GetPostsAsync();

            Log.Information($"Found {posts.Count()} posts");

            return Ok(posts);
        }

        [HttpGet("{postId}")]
        public async Task<IActionResult> GetPost(int postId)
        {
            Log.Information("GET /posts/{PostId} called", postId);

            var post = await _postService.GetPostByIdAsync(postId);

            if (post.id == 0)
            {
                Log.Warning("Post with ID {PostId} not found", postId);
                return NotFound();
            }

            Log.Information("Found post with ID {PostId}", postId);
            return Ok(post);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] PostDto post)
        {
            Log.Information("POST /posts called");

            var createdPost = await _postService.CreatePostAsync(post);

            Log.Information("Created post with ID {PostId}", createdPost.id);

            return Ok(createdPost);
        }

        [HttpPut("{postId}")]
        public async Task<IActionResult> UpdatePost(int postId, PostDto post)
        {
            Log.Information("PUT /posts/{PostId} called", postId);

            return Ok(await _postService.UpdatePostAsync(postId, post));
        }

        [HttpPatch("{postId}")]
        public async Task<IActionResult> PatchPost(int postId, object post)
        {
            Log.Information("PATCH /posts/{PostId} called", postId);

            return Ok(await _postService.PatchPostAsync(postId, post));
        }

        [HttpDelete("{postId}")]
        public async Task<IActionResult> DeletePost(int postId)
        {
            Log.Information("DELETE /posts/{PostId} called", postId);

            return Ok(await _postService.DeletePostAsync(postId));
        }
    }
}
