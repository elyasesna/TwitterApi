namespace TwitterApi.Data.Models
{
   public class PostModel
   {
      public string Content { get; set; }
      public List<string> Hashtags { get; set; } = new();
   }
}
