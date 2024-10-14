using Microsoft.EntityFrameworkCore;

namespace BlogPostsEntityFramework
{
  public class BlogService : IBlogService
  {
    private readonly BloggingContext _context;

    public BlogService(BloggingContext context)
    {
      _context = context;
    }

    public async Task<BlogListResultDto> FindBlogsAsync(int page, int size)
    {
      var totalCount = await _context.Blogs.CountAsync();

      var totalPages = (int)Math.Ceiling((double)totalCount / size);

      var skip = (page - 1) * size;
      var blogs =  await _context.Blogs
          .Skip(skip)
          .Take(size)
          .ToListAsync();

          return new BlogListResultDto{
            Blogs = blogs,
            TotalCount = totalCount,
            TotalPages = totalPages
          };
    }

    public async Task<Blog?> FindBlogByIdAsync(int blogId)
    {
      return await _context.Blogs.FindAsync(blogId);
    }

    public async Task<Blog> CreateBlogAsync(CreateUpdateBlogDto body)
    {
      var blog = new Blog { Url = body.Url };
      await _context.Blogs.AddAsync(blog);
      await _context.SaveChangesAsync();
      return blog;
    }

    public async Task<bool> UpdateBlogAsync(int blogId, CreateUpdateBlogDto body)
    {
      var blog = await _context.Blogs.FindAsync(blogId);
      if (blog == null) return false;

      blog.Url = body.Url;
      await _context.SaveChangesAsync();
      return true;
    }

    public async Task<bool> DeleteBlogAsync(int blogId)
    {
      var blog = await _context.Blogs.FindAsync(blogId);
      if (blog == null) {
        throw new BlogNotFoundException($"blog {blogId} not found");
      }

      _context.Blogs.Remove(blog);
      await _context.SaveChangesAsync();
      return true;
    }
  }

}