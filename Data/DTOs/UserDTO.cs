using System.Text.Json.Serialization;
using TwitterApi.Utilities;

namespace TwitterApi.Data.DTOs
{
   public class UserDTO
   {
      public string Id { get; set; }
      public bool IsConfirmed { get; set; }
      public string FirstName { get; set; }
      public string LastName { get; set; }
      public string UserName { get; set; }
      public string Email { get; set; }
      public string PhoneNumber { get; set; }
      [JsonIgnore]
      public string ProfileImagePath { get; set; }
      public string Avatar => ProfileImagePath.GetAvatarPath();
      public DateTime RegisteredAt { get; set; }
   }
}
