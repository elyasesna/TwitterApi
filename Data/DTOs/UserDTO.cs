namespace TwitterApi.Data.DTOs
{
   public class UserDTO
   {
      public string Id { get; set; }
      public bool IsConfirmed { get; set; }
      public string FistName { get; set; }
      public string LastName { get; set; }
      public string UserName { get; set; }
      public string Email { get; set; }
      public string PhoneNumber { get; set; }
      public string Avatar { get; set; }
      public DateTime RegisteredAt { get; set; }
   }
}
