using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwitterApi.Data.Entities
{
   public class Post
   {
      public long Id { get; set; }
      [Required]
      [MaxLength(500)]
      public string Content { get; set; }
      [ForeignKey("UserId")]
      [Required]
      public User User { get; set; }
      public string UserId { get; set; }
      public DateTime CreatedAt { get; set; } = DateTime.Now;
      public DateTime? LastUpdatedAt { get; set; }
      public ICollection<PostHashtag> PostTags { get; set; }
      public ICollection<Comment> Comments { get; set; }
      public ICollection<View> Views { get; set; }

   }
}
