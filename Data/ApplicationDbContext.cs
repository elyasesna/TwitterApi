using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TwitterApi.Data.Entities;

namespace TwitterApi.Data
{
   public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<User, Role, string>(options)
   {
      public DbSet<Post> Posts { get; set; }
      public DbSet<Hashtag> Hashtags { get; set; }

      protected override void OnModelCreating(ModelBuilder builder)
      {
         builder
             .Entity<User>()
             .Property(x => x.UserName)
             .HasColumnType("nvarchar(50)");

         builder
             .Entity<User>()
             .Property(x => x.NormalizedUserName)
             .HasColumnType("nvarchar(50)");

         builder
             .Entity<User>()
             .Property(x => x.Email)
             .HasColumnType("nvarchar(100)");

         builder
             .Entity<User>()
             .Property(x => x.NormalizedEmail)
             .HasColumnType("nvarchar(100)");

         builder
             .Entity<User>()
             .Property(x => x.PhoneNumber)
             .HasColumnType("nvarchar(11)");

         builder.Entity<User>()
            .Property(p => p.FullName)
            .HasComputedColumnSql("[FirstName] + ' ' + [LastName]");

         builder.Entity<Post>()
            .HasOne(p => p.User)
            .WithMany(u => u.Posts)
            .OnDelete(DeleteBehavior.NoAction);

         base.OnModelCreating(builder);
      }
   }
}
