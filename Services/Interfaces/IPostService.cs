using JsonPlaceholderProxyApi.Models;

namespace JsonPlaceholderProxyApi.Services.Interfaces
{
    public interface IPostService
    {
        Task<IEnumerable<PostDto>> GetPostsAsync();
        Task<PostDto> GetPostByIdAsync(int postId);
        Task<PostDto> CreatePostAsync(PostDto post);
        Task<PostDto> UpdatePostAsync(int postId, PostDto post);
        Task<PostDto> PatchPostAsync(int postId, object post);
        Task<bool> DeletePostAsync(int postId);
    }
}
