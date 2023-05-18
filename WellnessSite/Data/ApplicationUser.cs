using Microsoft.AspNetCore.Identity;

namespace WellnessSite.Data
{
    public class ApplicationUser : IdentityUser
    {
        public bool RequestedAdmin { get; set; }
        public string? Question1 { get; set; }
        public string? Question2 { get; set; }
        public string? Answer1 { get; set; }
        public string? Answer2 { get; set; }
        public string? Name { get; set; }
        public string? CookieState { get; set; }
    }
}
