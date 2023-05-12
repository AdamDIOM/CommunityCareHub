using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using WellnessSite.Data;
using WellnessSite.Models;

namespace WellnessSite.Pages
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _um;
        private readonly SignInManager<ApplicationUser> _sim;
        private readonly WellnessSiteContext _context;
        public Preferences p;
        public UsefulFunctions.CookiesOptions cookies = UsefulFunctions.CookiesOptions.Unknown;

        public IndexModel(SignInManager<ApplicationUser> sim, UserManager<ApplicationUser> um, WellnessSiteContext con)
        {
            _sim = sim;
            _um = um;
            _context = con;
        }

        public async Task OnGetAsync()
        {
            cookies = UsefulFunctions.IsCookiesEnabled(this);
            p = await UsefulFunctions.GetPreferences(_context, _um, _sim, User, this, cookies == UsefulFunctions.CookiesOptions.Enabled);
        }

        public async Task OnPostCookiesAsync(string choice)
        {
            cookies = UsefulFunctions.IsCookiesEnabled(this);
            p = await UsefulFunctions.GetPreferences(_context, _um, _sim, User, this, cookies == UsefulFunctions.CookiesOptions.Enabled);
            if (choice == "enabled") cookies = UsefulFunctions.CookiesOptions.Enabled;
            else cookies = UsefulFunctions.CookiesOptions.Disabled;
            Response.Cookies.Append("cookies", choice, new CookieOptions { Expires = DateTime.Now.AddDays(30) });
        }
    }
}