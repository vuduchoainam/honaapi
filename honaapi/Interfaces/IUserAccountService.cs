using honaapi.DTOs.jwt;
using static honaapi.Helpers.JwtResponse;

namespace honaapi.Interfaces
{
    public interface IUserAccountService
    {
        Task<GeneralResponse> RegisterAccount(RegisterDTO userDTO);
        Task<LoginResponse> LoginAccount(LoginDTO loginDTO);
    }
}
