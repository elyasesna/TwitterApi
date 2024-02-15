using Microsoft.AspNetCore.Mvc;
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
      {
         return Ok(await _userService.GetAllAsync());
      }

      
      [HttpGet("{id}")]
      public async Task<IActionResult> GetById(string id)
      {
         return Ok(await _userService.GetByIdAsync(id));
      }

      [HttpPost]
      public async Task<IActionResult> Create(UserModel user)
      {
         var result = await _userService.CreateAsync(user);
         return Ok(result);
      }

      [HttpPut("{id}")]
      public async Task<IActionResult> Update(string id, UserModel user)
      {
         var result = await _userService.UpdateAsync(id, user);
         return Ok(result);
      }

      [HttpDelete("{id}")]
      public async Task<IActionResult> Remove(string id)
      {
         return Ok(await _userService.Delete(id));
      }


      [HttpPost("/login")]
      public async Task<IActionResult> Login(LoginModel loginModel)
      {
         return Ok(await _userService.Login(loginModel));
      }
   }
}
