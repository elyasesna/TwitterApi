using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwitterApi.Data.Entities
{
   public class PostVisitors
   {
      public long Id { get; set; }

      [ForeignKey("UserId")]
      [Required]
      public User User { get; set; }
      public string UserId { get; set; }

      [ForeignKey("PostId")]
      public Post Post { get; set; }
      public long PostId { get; set; }
   }
}