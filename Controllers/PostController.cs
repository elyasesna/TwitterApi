using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TwitterApi.Contracts;
using TwitterApi.Data.Models;

namespace TwitterApi.Controllers
{
   [ApiController]
   [Route("api/post")]
   public class PostController : ControllerBase
   {

      private readonly ILogger<PostController> _logger;
      private readonly IPostService _postService;
      private IWebHostEnvironment _env;

      public PostController(
         ILogger<PostController> logger,
         IPostService postService,
         IWebHostEnvironment env)
      {
         _logger = logger;
         _postService = postService;
         _env = env;
      }

      [HttpGet]
      public async Task<IActionResult> GetAll()
         => Ok(await _postService.GetAllAsync());


      [HttpGet("{id:long}")]
      public async Task<IActionResult> GetById(long id)
         => Ok(await _postService.GetByIdAsync(id));

      [Authorize]
      [HttpPost]
      public async Task<IActionResult> Create(PostModel post)
      {
         string userId = HttpContext.User.Claims.First(p => p.Type == ClaimTypes.NameIdentifier).Value;
         return Ok(await _postService.CreateAsync(post, userId));
      }

      [Authorize]
      [HttpPut("{id:long}")]
      public async Task<IActionResult> Update(long id, PostModel post)
      {
         string userId = HttpContext.User.Claims.First(p => p.Type == ClaimTypes.NameIdentifier).Value;
         return Ok(await _postService.UpdateAsync(id, post, userId));
      }

      [Authorize]
      [HttpDelete("{id:long}")]
      public async Task<IActionResult> Remove(long id)
         => Ok(await _postService.Delete(id));

      [Authorize]
      [HttpPost("{id:long}/like")]
      public async Task<IActionResult> Like(long id)
         => Ok(await _postService.LikeAsync(id,
               HttpContext.User.Claims.First(p => p.Type == ClaimTypes.NameIdentifier).Value));

      [Authorize]
      [HttpPost("{id:long}/visit")]
      public async Task<IActionResult> Visit(long id)
         => Ok(await _postService.VisitAsync(id,
               HttpContext.User.Claims.First(p => p.Type == ClaimTypes.NameIdentifier).Value));

      [Authorize]
      [HttpPost("{id:long}/unlike")]
      public async Task<IActionResult> UnLike(long id)
         => Ok(await _postService.LikeAsync(id,
               HttpContext.User.Claims.First(p => p.Type == ClaimTypes.NameIdentifier).Value));

      [Authorize]
      [HttpPost("{id:long}/comment")]
      public async Task<IActionResult> Comment(long id, CreatePostCommentModel commnetModel)
               => Ok(await _postService.InsertCommentAsync(id, commnetModel,
                     HttpContext.User.Claims.First(p => p.Type == ClaimTypes.NameIdentifier).Value));

      [Authorize]
      [HttpDelete("{id:long}/comment/{commentId:long}")]
      public async Task<IActionResult> DeleteComment(long id, long commentId)
               => Ok(await _postService.DeleteCommentAsync(id, commentId,
                     HttpContext.User.Claims.First(p => p.Type == ClaimTypes.NameIdentifier).Value));

   }
}
