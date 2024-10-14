namespace BlogPostsEntityFramework
{
  public interface IBlogService
  {
    Task<BlogListResultDto> FindBlogsAsync(int page, int size);
    Task<Blog> FindBlogByIdAsync(int blogId);
    Task<Blog> CreateBlogAsync(CreateUpdateBlogDto body);
    Task<bool> UpdateBlogAsync(int blogId, CreateUpdateBlogDto body);
    Task<bool> DeleteBlogAsync(int blogId);
  }
}