using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using TwitterApi.Contracts;
using TwitterApi.Data;
using TwitterApi.Data.DTOs;
using TwitterApi.Data.Entities;
using TwitterApi.Data.Models;

namespace TwitterApi.Services
{
	public class PostService : BaseService, IPostService
	{
		//private readonly UserManager<User> _userManager;
		private readonly ISieveProcessor _sieveProcessor;

		public PostService(
			IUnitOfWork unitOfWork,
			IMapper mapper,
			ISieveProcessor sieveProcessor)
			: base(unitOfWork, mapper)
		{
			_sieveProcessor = sieveProcessor;
		}

		public async Task<PostDTO> CreateAsync(PostModel post, string userId)
		{
			if (!string.IsNullOrWhiteSpace(post.Content))
			{
				var postEntity = new Post
				{
					Content = post.Content,
					UserId = userId,
					Hashtags = []
				};

				foreach (var tag in post.Hashtags)
				{
					var hashtag = _unitOfWork.Get<Hashtag>().FirstOrDefault(p => p.Tag == tag);

					if (hashtag == null)
					{
						hashtag = new Hashtag
						{
							Tag = tag
						};
						await _unitOfWork.InsertAsync(hashtag);
					}
					else
					{
						hashtag.UsedCount += 1;
					}

					var postHashtag = new PostHashtags
					{
						Hashtag = hashtag
					};
					postEntity.Hashtags.Add(postHashtag);
				}

				await _unitOfWork.InsertAsync(postEntity);
				await _unitOfWork.CommityAsync();

				postEntity.User = _unitOfWork.Get<User>().FirstOrDefault(p => p.Id == userId);

				return _mapper.Map<PostDTO>(postEntity);
			}

			throw new Exception("Post content cannot be empty");
		}

		public async Task<PostDTO> UpdateAsync(long id, PostModel post, string userId)
		{
			if (string.IsNullOrWhiteSpace(post.Content))
				throw new Exception("Post content cannot be empty");

			var postEntity = await _unitOfWork.Get<Post>()
									.Include(p => p.User)
									.Include(p => p.Likes)
									.Include(p => p.Comments)
									.Include(p => p.Visitors)
									.Include(p => p.Hashtags)
										.ThenInclude(p => p.Hashtag)
									.FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId)
									?? throw new EntryPointNotFoundException();

			postEntity.Content = post.Content;

			foreach (var tag in post.Hashtags)
			{
				var hashtag = _unitOfWork.Get<Hashtag>().FirstOrDefault(p => p.Tag == tag);

				if (hashtag is null ||
					 !postEntity.Hashtags.Any(p => p.HashtagId == hashtag.Id))
				{
					if (hashtag is null)
					{
						hashtag = new Hashtag
						{
							Tag = tag
						};
						await _unitOfWork.InsertAsync(hashtag);
					}
					else
					{
						hashtag.UsedCount += 1;
					}

					var postHashtag = new PostHashtags
					{
						Hashtag = hashtag
					};

					postEntity.Hashtags.Add(postHashtag);
				}
			}

			foreach (var postHashtag in postEntity.Hashtags.Where(p =>
													!post.Hashtags.Contains(p.Hashtag.Tag)))
			{
				var hashtag = _unitOfWork.Get<Hashtag>().FirstOrDefault(p => p.Id == postHashtag.HashtagId);
				hashtag.UsedCount = hashtag.UsedCount > 0 ? hashtag.UsedCount - 1 : 0;

				postEntity.Hashtags.Remove(postHashtag);
			}

			await _unitOfWork.CommityAsync();

			return _mapper.Map<PostDTO>(postEntity);
		}

		public async Task<bool> DeleteAsync(long id, string userId)
		{
			var postEntity = await _unitOfWork.Get<Post>()
									.FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId)
									?? throw new EntryPointNotFoundException();
			_unitOfWork.Delete(postEntity);
			return await _unitOfWork.CommityAsync();
		}

		public async Task<PagedListDTO<PostDTO>> GetAllAsync(SieveModel sieveModel)
		{
			var result = _unitOfWork
				.Get<Post>()
				.AsNoTracking();

			//filtering & ordering
			result = _sieveProcessor.Apply(sieveModel, result, applyPagination: false);
			var count = await result.CountAsync();
			if (count > 0)
			{
				//pagination
				var records = _sieveProcessor
				  .Apply(sieveModel, result)
				  .ProjectTo<PostDTO>(_mapper.ConfigurationProvider)
				  .ToHashSet();

				return new PagedListDTO<PostDTO>
				{
					TotalCount = count,
					Records = records,
					CurrentPage = sieveModel.Page ?? 1,
					PageSize = sieveModel.PageSize ?? 15
				};
			}

			return new PagedListDTO<PostDTO>
			{
				TotalCount = 0,
				Records = new HashSet<PostDTO>(),
				CurrentPage = sieveModel.Page ?? 1,
				PageSize = sieveModel.PageSize ?? 15
			};
		}

		public Task<PostDTO> GetByIdAsync(long id)
		{
			return _unitOfWork
				.Get<Post>()
				.AsNoTracking()
				.Where(p => p.Id == id)
				.ProjectTo<PostDTO>(_mapper.ConfigurationProvider)
				.FirstOrDefaultAsync();
		}

		public async Task<bool> InsertCommentAsync(long id, CreatePostCommentModel commentModel, string userId)
		{
			var postEntity = await _unitOfWork.Get<Post>()
									.Include(p => p.Comments)
									.FirstOrDefaultAsync(p => p.Id == id)
									?? throw new EntryPointNotFoundException();

			var commentEntity = new PostComments
			{
				Content = commentModel.Content,
				UserId = userId
			};

			postEntity.Comments.Add(commentEntity);
			return await _unitOfWork.CommityAsync();

			/*Plan B
         //need to add DbSet<PostComments> into ApplicationDbContext
         var postEntity = await _unitOfWork.GetByIdAsync<Post>(postId)
                           ?? throw new EntryPointNotFoundException();
         var commentEntity = new PostComments
         {
            Content = content,
            UserId = userId,
            PostId = postId
         };
         _unitOfWork.InsertAsync(commentEntity);
         */
		}

		public async Task<bool> DeleteCommentAsync(long id, long commentId, string userId)
		{
			var postEntity = await _unitOfWork.Get<Post>()
									.Include(p => p.Comments)
									.FirstOrDefaultAsync(p => p.Id == id)
									?? throw new EntryPointNotFoundException();

			var comment = postEntity.Comments
								.FirstOrDefault(c => c.Id == commentId && c.UserId == userId)
				?? throw new EntryPointNotFoundException();

			postEntity.Comments.Remove(comment);
			return await _unitOfWork.CommityAsync();
		}

		public async Task<bool> LikeAsync(long postId, string userId)
		{
			var postEntity = await _unitOfWork.Get<Post>()
									.Include(p => p.Likes)
									.FirstOrDefaultAsync(p => p.Id == postId)
									?? throw new EntryPointNotFoundException();

			if (postEntity.Likes.Any(p => p.UserId == userId)) return true;

			var likeEntity = new PostLikes
			{
				UserId = userId
			};

			postEntity.Likes.Add(likeEntity);
			return await _unitOfWork.CommityAsync();
		}

		public async Task<bool> UnlikeAsync(long postId, string userId)
		{
			var postEntity = await _unitOfWork.Get<Post>()
						.Include(p => p.Likes)
						.FirstOrDefaultAsync(p => p.Id == postId)
						?? throw new EntryPointNotFoundException();

			var like = postEntity.Likes
								.FirstOrDefault(c => c.UserId == userId);
			//?? throw new EntryPointNotFoundException();

			if (like == null) return true;

			postEntity.Likes.Remove(like);
			return await _unitOfWork.CommityAsync();
		}

		public async Task<bool> VisitAsync(long postId, string userId)
		{
			var postEntity = await _unitOfWork.Get<Post>()
						.Include(p => p.Visitors)
						.FirstOrDefaultAsync(p => p.Id == postId)
						?? throw new EntryPointNotFoundException();

			if (postEntity.Visitors.Any(p => p.UserId == userId)) return true;

			var visitorEntity = new PostVisitors
			{
				UserId = userId
			};

			postEntity.Visitors.Add(visitorEntity);
			return await _unitOfWork.CommityAsync();
		}
	}
}
