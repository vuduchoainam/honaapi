using honaapi.DTOs.jwt;
using honaapi.Helpers;
using honaapi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace honaapi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserAccountService _userAccountService;
        public AccountController(IUserAccountService userAccountService)
        {
            _userAccountService = userAccountService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            var response = await _userAccountService.RegisterAccount(registerDTO);
            return StatusCodeResponse.SuccessResponse(response);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var token = await _userAccountService.LoginAccount(loginDTO);
            if (token == null)
            {
                return Unauthorized("Invalid credentials.");
            }
            return StatusCodeResponse.SuccessResponse(new { Token = token });
        }
    }
}
