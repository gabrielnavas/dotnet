namespace BlogPostsEntityFramework
{
    public class CreateUpdateBlogDto
    {
        public string Url { get; set; }
    }

    public class BlogDto {
        public int BlogId { get; set; }
        public string Url { get; set; }
    }

    public class BlogListResultDto
    {
        public List<Blog> Blogs { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }

    }
}