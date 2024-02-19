using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TwitterApi.Contracts;
using TwitterApi.Data;
using TwitterApi.Data.DTOs.User;
using TwitterApi.Data.Entities;
using TwitterApi.Data.Models.User;

namespace TwitterApi.Services
{
   public class UserService : BaseService, IUserService
   {
      private readonly UserManager<User> _userManager;

      public UserService(
         IUnitOfWork unitOfWork,
         UserManager<User> userManager)
         : base(unitOfWork)
      {
         _userManager = userManager;
      }

      public async Task<UserDTO> CreateAsync(UserModel user)
      {
         var userEntity = new User
         {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            UserName = user.UserName,
            PhoneNumber = user.PhoneNumber
         };
         var result = await _userManager.CreateAsync(userEntity);

         if (result.Succeeded)
         {
            return new UserDTO
            {
               PhoneNumber = userEntity.PhoneNumber,
               UserName = userEntity.UserName,
               Email = userEntity.Email,
               FistName = userEntity.FirstName,
               Id = userEntity.Id,
               IsConfirmed = userEntity.EmailConfirmed,
               LastName = userEntity.LastName
            };
         }
         throw new AccessViolationException();
      }

      public async Task<bool> Delete(string id)
      {
         var userEntity = await _unitOfWork.GetByIdAsync<User>(id);
         var result = await _userManager.DeleteAsync(userEntity);
         return result.Succeeded;
      }

      public Task<List<UserDTO>> GetAllAsync()
      {
         return _unitOfWork
            .Get<User>()
            .Select(p => new UserDTO
            {
               PhoneNumber = p.PhoneNumber,
               UserName = p.UserName,
               Email = p.Email,
               FistName = p.FirstName,
               Id = p.Id,
               IsConfirmed = p.EmailConfirmed,
               LastName = p.LastName
            })
            .ToListAsync();
      }

      public async Task<UserDTO> GetByIdAsync(string id)
      {
         var result = await _unitOfWork
            .GetByIdAsync<User>(id);

         return new UserDTO
         {
            PhoneNumber = result.PhoneNumber,
            UserName = result.UserName,
            Email = result.Email,
            FistName = result.FirstName,
            Id = result.Id,
            IsConfirmed = result.EmailConfirmed,
            LastName = result.LastName
         };
      }

      public async Task<UserDTO> UpdateAsync(string userId, UserModel user)
      {
         var userEntity = await _unitOfWork
            .GetByIdAsync<User>(userId) ?? throw new EntryPointNotFoundException();

         userEntity.PhoneNumber = user.PhoneNumber;
         userEntity.UserName = user.UserName;
         userEntity.FirstName = user.FirstName;
         userEntity.LastName = user.LastName;
         userEntity.Email = user.Email;

         var result = await _userManager.UpdateAsync(userEntity);

         if (result.Succeeded)
         {
            return new UserDTO
            {
               PhoneNumber = userEntity.PhoneNumber,
               UserName = userEntity.UserName,
               Email = userEntity.Email,
               FistName = userEntity.FirstName,
               Id = userEntity.Id,
               IsConfirmed = userEntity.EmailConfirmed,
               LastName = userEntity.LastName
            };
         }

         throw new EntryPointNotFoundException();
      }
   }
}
