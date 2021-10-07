using AutoMapper;
using BeDoHave.Application.Dtos;
using BeDoHave.Application.Interfaces;
using BeDoHave.Application.Specifications;
using BeDoHave.Data.AccessLayer.Interfaces;
using BeDoHave.Data.Core.Entities;
using BeDoHave.Shared.Entities;
using BeDoHave.Shared.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BeDoHave.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IAsyncRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public UserService(
            IAsyncRepository<User> userRepository, 
            IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task CreateUserAsync(CreateUserDto createUserDto)
        {
            var user = _mapper.Map<User>(createUserDto);

            await _userRepository.AddAsync(user);
        }

        public async Task CreateUserAsync(string identityId)
        {
            var user = await _userRepository.GetSingleBySpecAsync(
                new UserByIdentityIdSpecification(identityId));

            if (user is not null)
            {
                throw new ApiException("User already exists", 400);
            }

            user = new User
            {
                IdentityId = identityId
            };

            await _userRepository.AddAsync(user);
        }

        public async Task DeleteUserAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            
            if (user is null)
            {
                throw new ApiException($"User with id: {userId} not found", 404);
            }

            await _userRepository.DeleteAsync(user);
        }

        public async Task UpdateUserAsync(UpdateUserDto updateUserDto)
        {
            var user = await _userRepository.GetByIdAsync(updateUserDto.Id);

            if (user is null)
            {
                throw new ApiException("User not found", 404);
            }

            _mapper.Map(updateUserDto, user);
            await _userRepository.UpdateAsync(user);
        }

        public async Task<UserDto> GetUserByIdAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user is null)
            {
                throw new ApiException("User not found", 404);
            }

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> GetUserByIdentityId(string identityId)
        {
            var user = await _userRepository.GetSingleBySpecAsync(
              new UserByIdentityIdSpecification(identityId));

            if (user is null)
            {
                throw new ApiException("User not found", 404);
            }

            return _mapper.Map<UserDto>(user);
        }

        public async Task<IList<UserDto>> GetUsersAsync(PaginationParameters paginationParameters)
        {
            var users = await _userRepository.GetBySpecAsync(
                new UserSpecification(
                    u => u.FullName.Contains(paginationParameters.Keyword), 
                    start: paginationParameters.PageIndex * paginationParameters.PageSize, 
                    take: paginationParameters.PageSize, 
                    paginationParameters.OrderBy));

            return _mapper.Map<IList<UserDto>>(users);
        }
    }
}
