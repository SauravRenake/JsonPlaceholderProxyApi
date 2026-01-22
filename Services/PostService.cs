using System.Net.Http.Json;
using JsonPlaceholderProxyApi.Models;
using JsonPlaceholderProxyApi.Services.Interfaces;
using Serilog;

namespace JsonPlaceholderProxyApi.Services
{
    public class PostService : IPostService
    {
        private const string BaseUrl = "https://jsonplaceholder.typicode.com/posts";
        private readonly HttpClient _httpClient;

        public PostService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // public async Task<IEnumerable<PostDto>> GetPostsAsync()
        // {
        //     return await _httpClient.GetFromJsonAsync<IEnumerable<PostDto>>(BaseUrl)
        //            ?? Enumerable.Empty<PostDto>();
        // }

        public async Task<IEnumerable<PostDto>> GetPostsAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<IEnumerable<PostDto>>(BaseUrl)
                       ?? Enumerable.Empty<PostDto>();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error while fetching posts from JSONPlaceholder");
                throw;
            }
        }

        public async Task<PostDto> GetPostByIdAsync(int postId)
        {
            return await _httpClient.GetFromJsonAsync<PostDto>($"{BaseUrl}/{postId}")
                   ?? new PostDto();
        }

        public async Task<PostDto> CreatePostAsync(PostDto post)
        {
            var response = await _httpClient.PostAsJsonAsync(BaseUrl, post);
            return await response.Content.ReadFromJsonAsync<PostDto>()
                   ?? new PostDto();
        }

        public async Task<PostDto> UpdatePostAsync(int postId, PostDto post)
        {
            var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{postId}", post);
            return await response.Content.ReadFromJsonAsync<PostDto>()
                   ?? new PostDto();
        }

        public async Task<PostDto> PatchPostAsync(int postId, object post)
        {
            var response = await _httpClient.PatchAsJsonAsync($"{BaseUrl}/{postId}", post);
            return await response.Content.ReadFromJsonAsync<PostDto>()
                   ?? new PostDto();
        }

        public async Task<bool> DeletePostAsync(int postId)
        {
            var response = await _httpClient.DeleteAsync($"{BaseUrl}/{postId}");
            return response.IsSuccessStatusCode;
        }
    }
}
