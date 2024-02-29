using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TwitterApi.Data.Entities
{
   public class User : IdentityUser
   {
      [MaxLength(50)]
      public string FirstName { get; set; } = string.Empty;
      [MaxLength(50)]
      public string LastName { get; set; } = string.Empty;
      [MaxLength(150)]
      public string ProfileImagePath { get; set; } = string.Empty;

      public string FullName { get; set; }
      public DateTime RegisteredAt { get; set; } = DateTime.Now;
      public DateTime LastUpdatedAt { get; set; } = DateTime.Now;

      public ICollection<Post> Posts { get; set; }
   }
}
