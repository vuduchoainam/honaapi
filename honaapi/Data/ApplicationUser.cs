using Microsoft.AspNetCore.Identity;

namespace honaapi.Data
{
    public class ApplicationUser :IdentityUser
    {
        public string? Name { get; set; }
    }
}
