namespace TwitterApi.Data.DTOs
{
	public class PagedListDTO<T>
	{
		public int TotalCount { get; set; }
		public HashSet<T> Records { get; set; }
		public int CurrentPage { get; set; } = 1;
		public int PageSize { get; set; } = 15;
		public bool HasNextPage => CurrentPage * PageSize < TotalCount;
	}
}
