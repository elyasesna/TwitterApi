using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TwitterApi.Contracts;
using TwitterApi.Data.Models.User;

namespace TwitterApi.Controllers
{
   [ApiController]
   [Route("api/user")]
   public class UserController : ControllerBase
   {

      private readonly ILogger<UserController> _logger;
      private readonly IUserService _userService;

      public UserController(
         ILogger<UserController> logger,
         IUserService userService)
      {
         _logger = logger;
         _userService = userService;
      }

      [HttpGet]
      public async Task<IActionResult> GetAll()
         => Ok(await _userService.GetAllAsync());


      [HttpGet("{id}")]
      public async Task<IActionResult> GetById(string id)
         => Ok(await _userService.GetByIdAsync(id));

      [HttpPost]
      public async Task<IActionResult> Create(UserModel user)
         => Ok(await _userService.CreateAsync(user));

      [Authorize]
      [HttpPut("{id}")]
      public async Task<IActionResult> Update(string id, UserModel user)
         => Ok(await _userService.UpdateAsync(id, user));

      [HttpDelete("{id}")]
      public async Task<IActionResult> Remove(string id)
         => Ok(await _userService.Delete(id));

      [Authorize]
      [HttpGet("claims")]
      public IActionResult Claims()
         => Ok(HttpContext.User.Claims.First(p=> p.Type == ClaimTypes.Name).Value);
   }
}
