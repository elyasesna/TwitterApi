using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TwitterApi.Data.Entities;
using DNTPersianUtils.Core;

namespace TwitterApi.Data.DTOs
{
   public class PostDTO
   {
      public long Id { get; set; }
      public string Content { get; set; }
      public string UserFullName { get; set; }
      public string Username { get; set; }
      public DateTime CreatedAt { get; set; }
      public string CreatedAtFa => CreatedAt.ToShortPersianDateTimeString();
      public int LikeCount { get; set; }
      public int CommentCount { get; set; }
      public int VisitCount { get; set; }
   }
}