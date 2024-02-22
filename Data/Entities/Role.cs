using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TwitterApi.Data.Entities
{
   public class Role : IdentityRole
   {
      [MaxLength(50)]
      public string Title { get; set; }
   }
}
