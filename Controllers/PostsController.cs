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

        // Centralized accessor for CorrelationId
        private string CorrelationId =>
            HttpContext.Items[CorrelationIdKey]?.ToString() ?? "N/A";

        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            Log.Information(
                "Request started: GET /posts | CorrelationId={CorrelationId}",
                CorrelationId);

            var posts = await _postService.GetPostsAsync();

            Log.Information(
                "Request completed: GET /posts | ReturnedCount={Count} | CorrelationId={CorrelationId}",
                posts.Count(),
                CorrelationId);

            return Ok(posts);
        }

        [HttpGet("{postId}")]
        public async Task<IActionResult> GetPost(int postId)
        {
            Log.Information(
                "Request started: GET /posts/{PostId} | CorrelationId={CorrelationId}",
                postId,
                CorrelationId);

            var post = await _postService.GetPostByIdAsync(postId);

            if (post == null || post.id == 0)
            {
                Log.Warning(
                    "Post not found | PostId={PostId} | CorrelationId={CorrelationId}",
                    postId,
                    CorrelationId);

                return NotFound();
            }

            Log.Information(
                "Request completed: GET /posts/{PostId} | CorrelationId={CorrelationId}",
                postId,
                CorrelationId);

            return Ok(post);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] PostDto post)
        {
            Log.Information(
                "Request started: POST /posts | CorrelationId={CorrelationId}",
                CorrelationId);

            var createdPost = await _postService.CreatePostAsync(post);

            Log.Information(
                "Post created successfully | PostId={PostId} | CorrelationId={CorrelationId}",
                createdPost?.id,
                CorrelationId);

            return Ok(createdPost);
        }

        [HttpPut("{postId}")]
        public async Task<IActionResult> UpdatePost(int postId, [FromBody] PostDto post)
        {
            Log.Information(
                "Request started: PUT /posts/{PostId} | CorrelationId={CorrelationId}",
                postId,
                CorrelationId);

            var updatedPost = await _postService.UpdatePostAsync(postId, post);

            Log.Information(
                "Post updated successfully | PostId={PostId} | CorrelationId={CorrelationId}",
                postId,
                CorrelationId);

            return Ok(updatedPost);
        }

        [HttpPatch("{postId}")]
        public async Task<IActionResult> PatchPost(int postId, [FromBody] object post)
        {
            Log.Information(
                "Request started: PATCH /posts/{PostId} | CorrelationId={CorrelationId}",
                postId,
                CorrelationId);

            var patchedPost = await _postService.PatchPostAsync(postId, post);

            Log.Information(
                "Post patched successfully | PostId={PostId} | CorrelationId={CorrelationId}",
                postId,
                CorrelationId);

            return Ok(patchedPost);
        }

        [HttpDelete("{postId}")]
        public async Task<IActionResult> DeletePost(int postId)
        {
            Log.Information(
                "Request started: DELETE /posts/{PostId} | CorrelationId={CorrelationId}",
                postId,
                CorrelationId);

            var result = await _postService.DeletePostAsync(postId);

            Log.Information(
                "Post deleted | PostId={PostId} | Success={Success} | CorrelationId={CorrelationId}",
                postId,
                result,
                CorrelationId);

            return Ok(result);
        }
    }
}
