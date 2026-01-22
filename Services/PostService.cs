using System.Net.Http.Json;
using JsonPlaceholderProxyApi.Models;
using JsonPlaceholderProxyApi.Services.Interfaces;
using Serilog;
using Microsoft.AspNetCore.Http;

namespace JsonPlaceholderProxyApi.Services
{
    public class PostService : IPostService
    {
        private const string BaseUrl = "https://jsonplaceholder.typicode.com/posts";
        private const string CorrelationIdKey = "X-Correlation-Id";

        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PostService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        // Centralized CorrelationId access
        private string CorrelationId =>
            _httpContextAccessor.HttpContext?.Items[CorrelationIdKey]?.ToString() ?? "N/A";

        public async Task<IEnumerable<PostDto>> GetPostsAsync()
        {
            Log.Information(
                "Service started: Fetching all posts | CorrelationId={CorrelationId}",
                CorrelationId);

            try
            {
                var posts = await _httpClient.GetFromJsonAsync<IEnumerable<PostDto>>(BaseUrl)
                            ?? Enumerable.Empty<PostDto>();

                Log.Information(
                    "Service completed: Fetched {Count} posts | CorrelationId={CorrelationId}",
                    posts.Count(),
                    CorrelationId);

                return posts;
            }
            catch (Exception ex)
            {
                Log.Error(
                    ex,
                    "Service error: Failed to fetch posts | CorrelationId={CorrelationId}",
                    CorrelationId);

                throw;
            }
        }

        public async Task<PostDto> GetPostByIdAsync(int postId)
        {
            Log.Information(
                "Service started: Fetching post by id | PostId={PostId} | CorrelationId={CorrelationId}",
                postId,
                CorrelationId);

            var post = await _httpClient.GetFromJsonAsync<PostDto>($"{BaseUrl}/{postId}");

            if (post == null || post.id == 0)
            {
                Log.Warning(
                    "Service warning: Post not found | PostId={PostId} | CorrelationId={CorrelationId}",
                    postId,
                    CorrelationId);

                return new PostDto();
            }

            Log.Information(
                "Service completed: Post fetched | PostId={PostId} | CorrelationId={CorrelationId}",
                postId,
                CorrelationId);

            return post;
        }

        public async Task<PostDto> CreatePostAsync(PostDto post)
        {
            Log.Information(
                "Service started: Creating post | CorrelationId={CorrelationId}",
                CorrelationId);

            var response = await _httpClient.PostAsJsonAsync(BaseUrl, post);
            var createdPost = await response.Content.ReadFromJsonAsync<PostDto>()
                              ?? new PostDto();

            Log.Information(
                "Service completed: Post created | PostId={PostId} | CorrelationId={CorrelationId}",
                createdPost.id,
                CorrelationId);

            return createdPost;
        }

        public async Task<PostDto> UpdatePostAsync(int postId, PostDto post)
        {
            Log.Information(
                "Service started: Updating post | PostId={PostId} | CorrelationId={CorrelationId}",
                postId,
                CorrelationId);

            var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{postId}", post);
            var updatedPost = await response.Content.ReadFromJsonAsync<PostDto>()
                              ?? new PostDto();

            Log.Information(
                "Service completed: Post updated | PostId={PostId} | CorrelationId={CorrelationId}",
                postId,
                CorrelationId);

            return updatedPost;
        }

        public async Task<PostDto> PatchPostAsync(int postId, object post)
        {
            Log.Information(
                "Service started: Patching post | PostId={PostId} | CorrelationId={CorrelationId}",
                postId,
                CorrelationId);

            var response = await _httpClient.PatchAsJsonAsync($"{BaseUrl}/{postId}", post);
            var patchedPost = await response.Content.ReadFromJsonAsync<PostDto>()
                              ?? new PostDto();

            Log.Information(
                "Service completed: Post patched | PostId={PostId} | CorrelationId={CorrelationId}",
                postId,
                CorrelationId);

            return patchedPost;
        }

        public async Task<bool> DeletePostAsync(int postId)
        {
            Log.Information(
                "Service started: Deleting post | PostId={PostId} | CorrelationId={CorrelationId}",
                postId,
                CorrelationId);

            var response = await _httpClient.DeleteAsync($"{BaseUrl}/{postId}");

            Log.Information(
                "Service completed: Post deleted | PostId={PostId} | Success={Success} | CorrelationId={CorrelationId}",
                postId,
                response.IsSuccessStatusCode,
                CorrelationId);

            return response.IsSuccessStatusCode;
        }
    }
}
