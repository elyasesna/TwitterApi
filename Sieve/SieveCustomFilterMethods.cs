using Microsoft.EntityFrameworkCore;
using Sieve.Services;
using TwitterApi.Data.Entities;

namespace TwitterApi.Sieve
{
	public class SieveCustomFilterMethods : ISieveCustomFilterMethods
	{
		public IQueryable<Post> Hashtag(IQueryable<Post> source, string op, string[] values)
		{
			return source.Where(p => p.Hashtags.Any(q => q.Hashtag.Tag.Contains(values[0])));
		}
	}
}
