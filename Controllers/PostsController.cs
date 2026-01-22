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

        private const string CorrelationIdKey = "X-Correlation-Id";

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        private string CorrelationId =>
            HttpContext.Items[CorrelationIdKey]?.ToString() ?? "N/A";

        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            Log.Information(
                "HTTP GET /posts started | CorrelationId={CorrelationId}",
                CorrelationId);

            var posts = await _postService.GetPostsAsync();

            Log.Information(
                "HTTP GET /posts completed | ReturnedCount={Count} | CorrelationId={CorrelationId}",
                posts.Count(),
                CorrelationId);

            return Ok(posts);
        }

        [HttpGet("{postId}")]
        public async Task<IActionResult> GetPost(int postId)
        {
            Log.Information(
                "HTTP GET /posts/{PostId} started | CorrelationId={CorrelationId}",
                postId,
                CorrelationId);

            var post = await _postService.GetPostByIdAsync(postId);

            Log.Information(
                "HTTP GET /posts/{PostId} completed | CorrelationId={CorrelationId}",
                postId,
                CorrelationId);

            return Ok(post);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] PostDto post)
        {
            Log.Information(
                "HTTP POST /posts started | CorrelationId={CorrelationId}",
                CorrelationId);

            var createdPost = await _postService.CreatePostAsync(post);

            Log.Information(
                "HTTP POST /posts completed | PostId={PostId} | CorrelationId={CorrelationId}",
                createdPost.id,
                CorrelationId);

            return Ok(createdPost);
        }

        [HttpPut("{postId}")]
        public async Task<IActionResult> UpdatePost(int postId, [FromBody] PostDto post)
        {
            Log.Information(
                "HTTP PUT /posts/{PostId} started | CorrelationId={CorrelationId}",
                postId,
                CorrelationId);

            var updatedPost = await _postService.UpdatePostAsync(postId, post);

            Log.Information(
                "HTTP PUT /posts/{PostId} completed | CorrelationId={CorrelationId}",
                postId,
                CorrelationId);

            return Ok(updatedPost);
        }

        [HttpPatch("{postId}")]
        public async Task<IActionResult> PatchPost(int postId, [FromBody] object post)
        {
            Log.Information(
                "HTTP PATCH /posts/{PostId} started | CorrelationId={CorrelationId}",
                postId,
                CorrelationId);

            var patchedPost = await _postService.PatchPostAsync(postId, post);

            Log.Information(
                "HTTP PATCH /posts/{PostId} completed | CorrelationId={CorrelationId}",
                postId,
                CorrelationId);

            return Ok(patchedPost);
        }

        [HttpDelete("{postId}")]
        public async Task<IActionResult> DeletePost(int postId)
        {
            Log.Information(
                "HTTP DELETE /posts/{PostId} started | CorrelationId={CorrelationId}",
                postId,
                CorrelationId);

            var result = await _postService.DeletePostAsync(postId);

            Log.Information(
                "HTTP DELETE /posts/{PostId} completed | Success={Success} | CorrelationId={CorrelationId}",
                postId,
                result,
                CorrelationId);

            return Ok(result);
        }
    }
}
