using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using TwitterApi.Contracts;
using TwitterApi.Data.DTOs;
using TwitterApi.Data.Entities;
using TwitterApi.Data.Models;
using TwitterApi.Utilities;

namespace TwitterApi.Controllers
{
   [ApiController]
   [Route("api/user")]
   public class UserController : ControllerBase
   {
      private const string FILE_PATH = "user-avatars";

      private readonly ILogger<UserController> _logger;
      private readonly IUserService _userService;
      private IWebHostEnvironment _env;

      public UserController(
         ILogger<UserController> logger,
         IUserService userService,
         IWebHostEnvironment env)
      {
         _logger = logger;
         _userService = userService;
         _env = env;
      }

      [Authorize(Roles = Roles.Admin)]
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

      [Authorize]
      [HttpDelete("{id}")]
      public async Task<IActionResult> Remove(string id)
         => Ok(await _userService.Delete(id));

      [Authorize]
      [HttpGet("info")]
      public async Task<IActionResult> GetUserInfo()
         => Ok(await _userService.GetByIdAsync(
               HttpContext.User.Claims.First(p => p.Type == ClaimTypes.NameIdentifier).Value));


      [Authorize]
      [HttpPost("avatar")]
      public async Task<IActionResult> Post([FromForm] UserImageModel avatar)
      {
         string filePath = "";
         if (avatar?.Avatar.Length > 0)
         {
            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;

            if (avatar?.Avatar.Length > 512000) //image size bigger than 500kb is not allowed
            {
               throw new NotSupportedException("image size should be smaller than 500kb");
            }

            if (!Helpers.IsValidImageExtention(Path.GetExtension(avatar.Avatar.FileName)))
            {
               throw new NotSupportedException("image type is not supported");
            }

            filePath = Path.Combine(_env.WebRootPath,
                  FILE_PATH,
                  userId.Replace("-", "") + "-" +
                  DateTime.Now.Ticks.ToString() +
                  Path.GetExtension(avatar.Avatar.FileName));

            using var stream = new FileStream(filePath, FileMode.Create);
            await avatar.Avatar.CopyToAsync(stream);

            var (result, oldPath) = await _userService.UpdateAvatarAsync(userId, filePath);

            if (!string.IsNullOrEmpty(oldPath) &&
                 System.IO.File.Exists(oldPath))
            {
               try
               {
                  System.IO.File.Delete(oldPath);
               }
               catch { }
            }

            return Ok(filePath.GetAvatarPath());
         }

         return Ok(filePath);
      }

   }
}
