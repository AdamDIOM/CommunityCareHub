using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using WellnessSite.Data;
using WellnessSite.Models;

namespace WellnessSite.Pages.auth
{
    public class AdminLoginModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _um;
        private readonly SignInManager<ApplicationUser> _sim;
        private readonly WellnessSiteContext _context;
        public Preferences p;
        public IList<SecQues> sq;
        [BindProperty(SupportsGet = true)]
        public string UID { get; set; }
        public string? SQError { get; set; } = "";

        [BindProperty]
        [Display(Name = "Security Question")]
        public string Q { get; set; }
        [BindProperty]
        [Display(Name = "Answer")]
        public string A { get; set; }

        public AdminLoginModel(SignInManager<ApplicationUser> sim, UserManager<ApplicationUser> um, WellnessSiteContext con)
        {
            _sim = sim;
            _um = um;
            _context = con;
        }

        public async Task<IActionResult> OnGetAsync()
        {

            p = await UsefulFunctions.GetPreferences(_context, _um, _sim, User, this);

            if (_context.SecQues != null)
            {
                sq = await _context.SecQues.ToListAsync();
            }
            var u = await _um.FindByIdAsync(UID);
            if (u == null || u.Question1 == null || u.Question2 == null) return NotFound();
            if (DateTime.Now.Second % 2 == 0)
            {
                Q = u.Question1;
            }
            else
            {
                Q = u.Question2;
            }
            return Page();

        }

        public async Task<IActionResult> OnPostAsync(string UID, string Q)
        {

            var u = await _um.FindByIdAsync(UID);
            if (u == null || u.Question1 == null || u.Question2 == null || u.Answer1 == null || u.Answer2 == null)
            {
                return NotFound();
            }
            string ans = "§§§";
            if (Q == u.Question1) ans = u.Answer1;
            else if (Q == u.Question2) ans = u.Answer2;
          
            if(A == ans)
            {
                await _sim.SignInAsync(u, false);
                if (u != null && u.CookieState != null)
                {
                    Response.Cookies.Append(".colourSchemeCookie", u.CookieState, new CookieOptions { Expires = DateTime.Now.AddDays(30) });
                }
                return RedirectToPage("../Index");
            }

            p = await UsefulFunctions.GetPreferences(_context, _um, _sim, User, this);

            if (_context.SecQues != null)
            {
                sq = await _context.SecQues.ToListAsync();
            }
            if (u == null || u.Question1 == null || u.Question2 == null) return NotFound();
            if (DateTime.Now.Second % 2 == 0)
            {
                Q = u.Question1;
            }
            else
            {
                Q = u.Question2;
            }
            //return Page();
            return RedirectToPage("../Index");
        }
    }
}
