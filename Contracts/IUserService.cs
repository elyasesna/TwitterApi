using TwitterApi.Data.DTOs.User;
using TwitterApi.Data.Entities;
using TwitterApi.Data.Models.User;

namespace TwitterApi.Contracts
{
   public interface IUserService
   {
      public Task<List<UserDTO>> GetAllAsync();
      public Task<UserDTO> GetByIdAsync(string id);
      public Task<UserDTO> CreateAsync(UserModel user);
      public Task<UserDTO> UpdateAsync(string userId, UserModel user);
      public Task<bool> Delete(string id);
   }
}
