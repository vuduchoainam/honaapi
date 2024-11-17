using honaapi.DTOs.jwt;
using honaapi.Helpers;
using honaapi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        [HttpGet("Profile")]
        [Authorize]
        public async Task<IActionResult> GetUserProfile()
        {
            // Lấy thông tin user từ Claims trong token
            var userId = User.FindFirstValue("id");
            if (userId == null)
            {
                return Unauthorized("Invalid token or user not found.");
            }

            // Tìm user bằng ID từ cơ sở dữ liệu
            var user = await _userAccountService.GetUserProfileById(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Trả về thông tin profile của user
            return StatusCodeResponse.SuccessResponse(new
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            });
        }
    }
}
