using AutoMapper;
using BeDoHave.Application.Dtos;
using BeDoHave.Application.Interfaces;
using BeDoHave.Shared.Dtos;
using BeDoHave.Shared.Interfaces;
using System.Threading.Tasks;

namespace BeDoHave.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountService(IAuthService authService, 
            IUserService userService, 
            ITokenService tokenService,
            IMapper mapper)
        {
            _authService = authService;
            _userService = userService;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public async Task<TokenDto> LoginAsync(LoginDto loginDto)
        {
            return await _authService.LoginAsync(loginDto);
        }

        public async Task<TokenDto> RegisterAsync(RegisterDto registerDto)
        {
            //TODO: Use transaction (implement UOW)
            var identityId = await _authService.CreateIdentityAsync(registerDto);

            await _userService.CreateUserAsync(identityId);

            return await _authService.LoginAsync(_mapper.Map<LoginDto>(registerDto));
        }

        public async Task<TokenDto> RevokeTokenAsync(RevokeTokenDto revokeTokenDto)
        {
            return await _authService.RevokeAsync(revokeTokenDto);
        }

        //TODO: Forgot password

        public async Task<string> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
        {
            return await _authService.ForgotPasswordAsync(forgotPasswordDto);
        }

        public async Task ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            await _authService.ResetPasswordAsync(resetPasswordDto);
        }

        public async Task<UserWithEmailDto> GetAuthenticatedUserAsync(string token)
        {
            var (Email, IdentityId) = _tokenService.GetUserClaimsFromToken(token);            
            var user = await _userService.GetUserByIdentityId(IdentityId);
            var userWithEmail = _mapper.Map<UserWithEmailDto>(user);

            userWithEmail.Email = Email;

            return userWithEmail;
        }
    }
}