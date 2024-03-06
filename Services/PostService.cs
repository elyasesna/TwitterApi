using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using TwitterApi.Contracts;
using TwitterApi.Data;
using TwitterApi.Data.DTOs;
using TwitterApi.Data.Entities;
using TwitterApi.Data.Models;
using TwitterApi.Utilities;

namespace TwitterApi.Services
{
   public class PostService : BaseService, IPostService
   {
      private readonly UserManager<User> _userManager;

      public PostService(
         IUnitOfWork unitOfWork,
         IMapper mapper)
         : base(unitOfWork, mapper)
      {
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

            return _mapper.Map<PostDTO>(postEntity);
         }

         throw new Exception("Post content cannot be empty");
      }

      public async Task<bool> Delete(long id)
      {
         var postEntity = await _unitOfWork.GetByIdAsync<Post>(id)
                           ?? throw new EntryPointNotFoundException();
         _unitOfWork.Delete(postEntity);
         return await _unitOfWork.CommityAsync();
      }

      public Task<List<PostDTO>> GetAllAsync()
      {
         return _unitOfWork
            .Get<Post>()
            .ProjectTo<PostDTO>(_mapper.ConfigurationProvider)
            .ToListAsync();
      }

      public Task<PostDTO> GetByIdAsync(long id)
      {
         return _unitOfWork
            .Get<Post>()
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
                        .FirstOrDefault(c => c.UserId == userId)
            ?? throw new EntryPointNotFoundException();

         postEntity.Likes.Remove(like);
         return await _unitOfWork.CommityAsync();
      }

      public Task<PostDTO> UpdateAsync(long id, PostModel post, string userId)
      {
         throw new NotImplementedException();
      }

      public async Task<bool> VisitAsync(long postId, string userId)
      {
         var postEntity = await _unitOfWork.Get<Post>()
                  .Include(p => p.Visitors)
                  .FirstOrDefaultAsync(p => p.Id == postId)
                  ?? throw new EntryPointNotFoundException();

         var visitorEntity = new PostVisitors
         {
            UserId = userId
         };

         postEntity.Visitors.Add(visitorEntity);
         return await _unitOfWork.CommityAsync();
      }
   }
}
