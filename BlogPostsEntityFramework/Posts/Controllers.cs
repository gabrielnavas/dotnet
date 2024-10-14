using Microsoft.AspNetCore.Mvc;

namespace BlogPostsEntityFramework
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] CreateUpdatePostDto dto)
        {
            try
            {
                await _postService.CreatePost(dto);
                return Created();
            }
            catch (BlogNotFoundException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("blog/{blogId}")]
        public async Task<IActionResult> FindPosts(int blogId, [FromQuery] int page = 0, [FromQuery] int size = 10, [FromQuery] string query = "")
        {
            try
            {
                var posts = await _postService.FindPosts(blogId, page, size, query);
                return Ok(new { posts });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("{postId}")]
        public async Task<IActionResult> FindPost(int postId)
        {
            try
            {
                var post = await _postService.FindPost(postId);
                return Ok(new { post });
            }
            catch (PostNotFoundException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }

        }
    }
}
