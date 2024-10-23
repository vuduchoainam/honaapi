namespace honaapi.Helpers
{
    public class JwtResponse
    {
        public record class GeneralResponse(bool isError, string Message);
        public record class LoginResponse(bool isError, string Token, string Message);
    }
}
