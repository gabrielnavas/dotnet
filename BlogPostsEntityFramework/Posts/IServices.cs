using System.Drawing;

namespace BlogPostsEntityFramework
{
  public interface IPostService
  {
    Task CreatePost(CreateUpdatePostDto data);
    Task<Post> FindPost(int postId);
    Task<List<PostDto>> FindPosts(int blogId, int page, int size, string query);
  }
}