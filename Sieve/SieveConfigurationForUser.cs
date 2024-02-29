using Sieve.Services;
using TwitterApi.Data.Entities;

namespace TwitterApi.Sieve
{
   public class SieveConfigurationForUser : ISieveConfiguration
   {
      public void Configure(SievePropertyMapper mapper)
      {
         mapper.Property<User>(p => p.UserName)
            .CanSort()
            .CanFilter();
      }
   }
}
