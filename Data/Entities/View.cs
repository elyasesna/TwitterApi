using System.ComponentModel.DataAnnotations.Schema;

namespace TwitterApi.Data.Entities
{
   public class View
   {
      public long Id { get; set; }

      [ForeignKey("UserId")]
      public User User { get; set; }
      public string UserId { get; set; }

      [ForeignKey("PostId")]
      public Post Post { get; set; }
      public long PostId { get; set; }
   }
}