using Sieve.Services;
using TwitterApi.Data.Entities;

namespace TwitterApi.Sieve
{
	public class SieveConfigurationForPost : ISieveConfiguration
	{
		public void Configure(SievePropertyMapper mapper)
		{
			mapper.Property<Post>(p => p.Content)
				.CanFilter();

			mapper.Property<Post>(p => p.User.FullName)
				.CanFilter()
				.HasName("UserName");

			mapper.Property<Post>(p => p.CreatedAt)
				.CanFilter()
				.CanSort()
				.HasName("CreatedDatetime");

			mapper.Property<Post>(p => p.LastUpdatedAt)
				.CanFilter()
				.CanSort()
				.HasName("UpdatedDatetime");
		}
	}
}
