using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using TwitterApi.Data.Entities;
using TwitterApi.Utilities;
using Microsoft.EntityFrameworkCore;

namespace TwitterApi.Data
{
   public class SeedData
   {
      public static void Initialize(WebApplication app)
      {
         using var scope = app.Services.CreateScope();

         var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
         db.Database.EnsureCreated();
         db.Database.Migrate();

         var roleManager = scope.ServiceProvider.GetService<RoleManager<Role>>();

         List<(string name, string title)> roles = [
            (Roles.Admin, "سوپرادمین")
         ];

         foreach (var (name, title) in roles)
         {
            if (!roleManager.Roles.Any(r => r.Name == name))
            {
               roleManager.CreateAsync(new Role
               {
                  Name = name,
                  Title = title,
                  NormalizedName = name.ToUpper(),
                  ConcurrencyStamp = Guid.NewGuid().ToString()
               }).Wait();
            }
         }

         var userManager = scope.ServiceProvider.GetService<UserManager<User>>();
         var user = new User
         {
            FirstName = "Elyas",
            LastName = "Esna",
            Email = "elyasesnaashari@yahoo.com",
            UserName = "elyasnew",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true
         };

         if (!userManager.Users.Any(u => u.UserName == user.UserName))
         {
            var result = userManager.CreateAsync(user, "Aa@123456").GetAwaiter().GetResult();
            if (result.Succeeded)
            {
               userManager.AddToRoleAsync(user, roles[0].name).Wait();
            }
         }
      }
   }
}
