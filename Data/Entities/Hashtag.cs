using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwitterApi.Data.Entities
{
   public class Hashtag
   {
      public long Id { get; set; }
      [Required]
      [MaxLength(50)]
      public string Tag { get; set; }
   }
}