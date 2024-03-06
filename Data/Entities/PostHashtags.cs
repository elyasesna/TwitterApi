using System.ComponentModel.DataAnnotations.Schema;

namespace TwitterApi.Data.Entities
{
   public class PostHashtags
   {
      public long Id { get; set; }

      [ForeignKey("PostId")]
      public Post Post { get; set; }
      public long PostId { get; set; }

      [ForeignKey("HashtagId")]
      public Hashtag Hashtag { get; set; }
      public long HashtagId { get; set; }
   }
}
