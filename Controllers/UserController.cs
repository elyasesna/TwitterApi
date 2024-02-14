using Microsoft.AspNetCore.Mvc;

namespace TwitterApi.Controllers
{
   [ApiController]
   [Route("api/user")]
   public class UserController : ControllerBase
   {

      private readonly ILogger<UserController> _logger;

      public UserController(ILogger<UserController> logger)
      {
         _logger = logger;
      }

   }
}
