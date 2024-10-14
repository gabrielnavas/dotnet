
using Microsoft.EntityFrameworkCore;

namespace BlogPostsEntityFramework
{
  public class PostService : IPostService
  {

    private readonly BloggingContext _context;

    public PostService(BloggingContext context)
    {
      _context = context;
    }

    public async Task CreatePost(CreateUpdatePostDto dto)
    {
      var blog = await _context.Blogs.FindAsync(dto.BlogId);

      if (blog == null)
      {
        throw new BlogNotFoundException($"Blog {dto.BlogId} not found");
      }

      blog.Posts.Add(new Post { Title = dto.Title, Content = dto.Content });
      await _context.SaveChangesAsync();
    }

    public async Task DeletePost(int postId)
    {
      var post = await FindPost(postId);
      _context.Remove(post);
      await _context.SaveChangesAsync();
    }


    public async Task<List<PostDto>> FindPosts(int blogId, int page, int size, string query)
    {
      var skip = (page - 1) * size;
      var posts = await _context.Posts
          .Select(post => new PostDto
          {
            PostId = post.PostId,
            BlogId = post.BlogId,
            Title = post.Title,
            Content = post.Content
          })
          .Where(post => post.BlogId == blogId)
          .Skip(skip)
          .Take(size)
          .ToListAsync();
      return posts;
    }

    public async Task<Post> FindPost(int postId)
    {
      var post = await _context.Posts.FindAsync(postId);
      if (post == null)
      {
        throw new PostNotFoundException($"post {postId} not found");
      }
      return post;
    }
  }
}