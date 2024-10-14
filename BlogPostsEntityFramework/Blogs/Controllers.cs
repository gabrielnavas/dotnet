using Microsoft.AspNetCore.Mvc;

namespace BlogPostsEntityFramework
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogController : ControllerBase
    {
        private readonly BloggingContext _context;
        private readonly IBlogService _blogService;

        public BlogController(
            BloggingContext context,
            IBlogService blogService
        )
        {
            _context = context;
            _blogService = blogService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBlog([FromBody] CreateUpdateBlogDto body)
        {
            try
            {
                await _blogService.CreateBlogAsync(body);
                return Created();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> FindBlogs([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            try
            {
                var blogsResults = await _blogService.FindBlogsAsync(page, size);
                return Ok(blogsResults);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("{blogId}")]
        public async Task<IActionResult> FindBlog(int blogId)
        {
            try
            {
                var blog = await _blogService.FindBlogByIdAsync(blogId);
                return Ok(new
                {
                    blog = new BlogDto { BlogId = blog.BlogId, Url = blog.Url }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPatch("{blogId}")]
        public async Task<IActionResult> UpdateBlog(int blogId, [FromBody] CreateUpdateBlogDto dto)
        {
            try
            {
                await _blogService.UpdateBlogAsync(blogId, dto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("{blogId}")]
        public async Task<IActionResult> DeleteBlog(int blogId)
        {
            try
            {
                await _blogService.DeleteBlogAsync(blogId);
                return NoContent();
            }
              catch (BlogNotFoundException ex)
            {
                return StatusCode(400, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
