using Sieve.Models;
using TwitterApi.Data.DTOs;
using TwitterApi.Data.Entities;
using TwitterApi.Data.Models;

namespace TwitterApi.Contracts
{
   public interface IPostService
   {
      public Task<PagedListDTO<PostDTO>> GetAllAsync(SieveModel sieveModel);
      public Task<PostDTO> GetByIdAsync(long id);
      public Task<PostDTO> CreateAsync(PostModel post, string userId);
      public Task<PostDTO> UpdateAsync(long id, PostModel post, string userId);
      public Task<bool> Delete(long id);

      public Task<bool> LikeAsync(long postId, string userId);
      public Task<bool> UnlikeAsync(long postId, string userId);
      public Task<bool> InsertCommentAsync(long id, CreatePostCommentModel commentModel, string userId);
      public Task<bool> DeleteCommentAsync(long id, long commentId, string userId);
      public Task<bool> VisitAsync(long postId, string userId);
   }
}
