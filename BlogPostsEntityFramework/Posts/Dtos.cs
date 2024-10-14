namespace BlogPostsEntityFramework
{
    public class CreateUpdatePostDto
    {
        public int BlogId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }

    public class PostDto
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int BlogId {get;set;}
    }

}