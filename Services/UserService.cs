using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TwitterApi.Contracts;
using TwitterApi.Data;
using TwitterApi.Data.DTOs;
using TwitterApi.Data.Entities;
using TwitterApi.Data.Models;
using TwitterApi.Utilities;

namespace TwitterApi.Services
{
   public class UserService : BaseService, IUserService
   {
      private readonly UserManager<User> _userManager;
      private readonly IWebHostEnvironment _env;

      public UserService(
         IUnitOfWork unitOfWork,
         IMapper mapper,
         UserManager<User> userManager,
         IWebHostEnvironment env)
         : base(unitOfWork, mapper)
      {
         _userManager = userManager;
         _env = env;
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
            return _mapper.Map<User, UserDTO>(userEntity);
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
            .ProjectTo<UserDTO>(_mapper.ConfigurationProvider)
            .ToListAsync();
      }

      public async Task<UserDTO> GetByIdAsync(string id)
      {
         //var result = await _unitOfWork
         //   .Get<User>()
         //   .Where(p => p.Id == id)
         //   .ProjectTo<UserDTO>(_mapper.ConfigurationProvider)
         //   .FirstOrDefaultAsync();

         var result = await _userManager.FindByIdAsync(id) ?? throw new EntryPointNotFoundException();

         return _mapper.Map<User, UserDTO>(result);
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
            return _mapper.Map<User, UserDTO>(userEntity);
         }

         throw new EntryPointNotFoundException();
      }

      public async Task<(bool result, string oldPath)> UpdateAvatarAsync(string userId, string filePath)
      {
         var userEntity = await _unitOfWork
            .GetByIdAsync<User>(userId) ?? throw new EntryPointNotFoundException();

         string oldPath = userEntity.ProfileImagePath;
         userEntity.ProfileImagePath = filePath;

         return (await _unitOfWork.CommityAsync(), oldPath);
      }
   }
}
