namespace TwitterApi.Utilities
{
   public static class Helpers
   {
      public static string GetRandomString(int length)
      {
         const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
         var random = new Random();
         return new string(Enumerable.Repeat(chars, length)
                        .Select(s => s[random.Next(s.Length)]).ToArray());
      }

      public static bool IsValidImageExtention(string extention)
      {
         return new List<string> { ".jpg", ".jpeg", ".png", ".webp" }.Contains(
             extention.ToLower()
         );
      }

      public static string GetAvatarPath(this string path, IWebHostEnvironment env)
      {
         return path.Replace(env.WebRootPath, "").Replace(@"\", "/");
      }
   }
}
