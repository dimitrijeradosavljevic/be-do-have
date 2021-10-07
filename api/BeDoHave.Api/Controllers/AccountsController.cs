using BeDoHave.Api.Binders;
using BeDoHave.Application.Interfaces;
using BeDoHave.Shared.Dtos;
using BeDoHave.Shared.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BeDoHave.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ITokenService _tokenService;

        public AccountsController(
            IAccountService accountService,
            ITokenService tokenService)
        {
            _accountService = accountService;
            _tokenService = tokenService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var tokenDto = await _accountService.LoginAsync(loginDto);

            return Ok(tokenDto);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var tokenDto = await _accountService.RegisterAsync(registerDto);

            return Ok(tokenDto);
        }

        [AllowAnonymous]
        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeAccessToken([ModelBinder(typeof(RevokeTokenModelBinder))] RevokeTokenDto revokeTokenDto)
        {
            var tokenDto = await _accountService.RevokeTokenAsync(revokeTokenDto);

            return Ok(tokenDto);
        }

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
        {
            var token = await _accountService.ForgotPassword(forgotPasswordDto);

            return Ok(token);
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ResetPassword([ModelBinder(typeof(ResetPasswordModelBinder))] ResetPasswordDto resetPasswordDto)
        {
            await _accountService.ResetPasswordAsync(resetPasswordDto);

            return Ok();
        }

        [Authorize]
        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Logout()
        {
            if (HttpContext.Request.Headers.TryGetValue("Authorization", out var token) && _tokenService.InvalidateOrCheckAccessToken(token))
            {
                return Ok();
            }

            return NotFound("Token does not exist.");
        }
        
        [Authorize]
        [HttpGet("current-user")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CurrentUser()
        {
            if (HttpContext.Request.Headers.TryGetValue("Authorization", out var token))
            {
                var currentUser = await _accountService.GetAuthenticatedUserAsync(token);

                return Ok(currentUser);
            }

            return NotFound("Token does not exist.");
        }
    }
}
