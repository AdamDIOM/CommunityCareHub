using Microsoft.AspNetCore.Identity;

namespace WellnessSite.Data
{
    public class ApplicationUser : IdentityUser
    {
        public bool RequestedAdmin { get; set; }
    }
}
