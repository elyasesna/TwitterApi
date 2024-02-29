using Microsoft.EntityFrameworkCore;
using Sieve.Services;
using TwitterApi.Data.Entities;

namespace TwitterApi.Sieve
{
   public class SieveCustomFilterMethods: ISieveCustomFilterMethods
   {
      public IQueryable<User> Search(IQueryable<User> source, string op, string[] values)
      {
         return source.Where(p => p.FullName.Contains(values[0]));
      }
   }
}
