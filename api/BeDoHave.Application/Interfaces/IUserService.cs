using BeDoHave.Application.Dtos;
using BeDoHave.Shared.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BeDoHave.Application.Interfaces
{
    public interface IUserService
    {
        Task<IList<UserDto>> GetUsersAsync(PaginationParameters paginationParameters);
        Task<UserDto> GetUserByIdAsync(int userId);
        Task<UserDto> GetUserByIdentityId(string identityId);
        Task CreateUserAsync(CreateUserDto createUserDto);
        Task CreateUserAsync(string identityId);
        Task UpdateUserAsync(UpdateUserDto updateUserDto);
        Task DeleteUserAsync(int userId);
    }
}
