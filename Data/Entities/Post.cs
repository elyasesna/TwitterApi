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
      public ICollection<PostHashtags> Hashtags { get; set; }
      public ICollection<PostComments> Comments { get; set; }
      public ICollection<PostVisitors> Visitors { get; set; }
      public ICollection<PostLikes> Likes { get; set; }

      [ForeignKey("RePostId")]
      public Post RePost { get; set; }
      public long? RePostId { get; set; }
   }
}
