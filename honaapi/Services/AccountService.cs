using honaapi.Data;
using honaapi.DTOs.jwt;
using honaapi.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using static honaapi.Helpers.JwtResponse;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace honaapi.Services
{
    public class AccountService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration config) : IUserAccountService
    {
        public async Task<GeneralResponse> RegisterAccount(RegisterDTO registerDTO)
        {
            if (registerDTO is null) return new GeneralResponse(true, "Model is empty");
            var newUser = new ApplicationUser()
            {
                UserName = registerDTO.Username,
                Email = registerDTO.Email
            };
            var user = await userManager.FindByEmailAsync(newUser.Email);
            if (user is not null) return new GeneralResponse(true, "User registered already");

            var createUser = await userManager.CreateAsync(newUser!, registerDTO.Password);
            if (!createUser.Succeeded) return new GeneralResponse(true, "Error occured.. please try again");

            //Assign Default Role : Admin to first registrar; rest is user
            var checkAdmin = await roleManager.FindByNameAsync("Admin");
            if (checkAdmin is null)
            {
                await roleManager.CreateAsync(new IdentityRole() { Name = "Admin" });
                await userManager.AddToRoleAsync(newUser, "Admin");
                return new GeneralResponse(false, "Account Created");
            }
            else
            {
                var checkUser = await roleManager.FindByNameAsync("User");
                if (checkUser is null)
                    await roleManager.CreateAsync(new IdentityRole() { Name = "User" });

                await userManager.AddToRoleAsync(newUser, "User");
                return new GeneralResponse(false, "Account Created");
            }
        }

        public async Task<LoginResponse> LoginAccount(LoginDTO loginDTO)
        {
            if (loginDTO == null)
                return new LoginResponse(true, null!, "Login container is empty");

            var getUser = await userManager.FindByNameAsync(loginDTO.Username);
            if (getUser is null)
                return new LoginResponse(true, null!, "User not found");

            bool checkUserPasswords = await userManager.CheckPasswordAsync(getUser, loginDTO.Password);
            if (!checkUserPasswords)
                return new LoginResponse(true, null!, "Invalid username/password");

            var getUserRole = await userManager.GetRolesAsync(getUser);
            var userSession = new UserSession(getUser.Id, getUser.UserName, getUser.Email, getUserRole.First());
            string token = GenerateToken(userSession);
            return new LoginResponse(false, token!, "Login completed");
        }

        private string GenerateToken(UserSession user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
            {
                new Claim("id", user.Id),
                new Claim("name", user.Name),
                new Claim("email", user.Email),
                new Claim("role", user.Role)
            };
            var token = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
