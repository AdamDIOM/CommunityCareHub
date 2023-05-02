using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using WellnessSite.Data;
using WellnessSite.Models;
using Microsoft.EntityFrameworkCore;

namespace WellnessSite.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public class ErrorModel : PageModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        private readonly ILogger<ErrorModel> _logger;

        private readonly UserManager<ApplicationUser> _um;
        private readonly SignInManager<ApplicationUser> _sim;
        private readonly WellnessSiteContext _context;
        private IList<Preferences> prefs;
        public Preferences p;

        public ErrorModel(ILogger<ErrorModel> logger, SignInManager<ApplicationUser> sim, UserManager<ApplicationUser> um, WellnessSiteContext con)
        {
            _logger = logger;
            _sim = sim;
            _um = um;
            _context = con;
        }

        public async Task OnGetAsync()
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            if (_context.Preferences != null)
            {
                prefs = await _context.Preferences.ToListAsync();
            }
            ApplicationUser u = await _um.GetUserAsync(User);

            if (_sim.IsSignedIn(User) && prefs.FirstOrDefault(p => p.UserID == u.Id) != null)
            {
                p = prefs.FirstOrDefault(p => p.UserID == u.Id)!;
            }
            else
            {
                p = new Preferences("u");
                if (Request.Cookies["user"] == null)
                {
                    Response.Cookies.Append("user", _context.Preferences.Count().ToString(), new CookieOptions { Expires = DateTime.Now.AddDays(30) });
                    _context.Preferences.Add(new Preferences("usr-" + _context.Preferences.Count().ToString()));
                    await _context.SaveChangesAsync();
                }
                else
                {
                    string uID = "usr-" + Request.Cookies["user"]!;

                    p = prefs.FirstOrDefault(p => p.UserID == uID)!;

                }
            }
        }
    }
}