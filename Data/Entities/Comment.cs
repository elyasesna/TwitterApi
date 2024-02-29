using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwitterApi.Data.Entities
{
   public class Comment
   {
      public long Id { get; set; }
      [Required]
      [MaxLength(250)]
      public string Content { get; set; }
      [ForeignKey("UserId")]
      [Required]
      public User User { get; set; }
      public string UserId { get; set; }

      [ForeignKey("PostId")]
      public Post Post { get; set; }
      public long PostId { get; set; }
   }
}