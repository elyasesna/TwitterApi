using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;
using System.Xml.Linq;

namespace TwitterApi.Sieve
{
   public class ApplicationSieveProcessor : SieveProcessor
   {
      public ApplicationSieveProcessor(
         IOptions<SieveOptions> options,
         ISieveCustomFilterMethods customFilterMethods) : base(options, customFilterMethods)
      {
      }

      protected override SievePropertyMapper MapProperties(SievePropertyMapper mapper)
      {
         return mapper.ApplyConfigurationsFromAssembly(typeof(ApplicationSieveProcessor).Assembly);
      }
   }
}
