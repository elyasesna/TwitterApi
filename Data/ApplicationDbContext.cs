using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TwitterApi.Data
{
   public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
   {
   }
}
