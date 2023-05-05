using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WellnessSite.Data;
using WellnessSite.Models;

namespace WellnessSite.Pages.Services
{
    public class ServiceModel : PageModel
    {
        private readonly WellnessSiteContext _context;
        private UserManager<ApplicationUser> _um;
        public ApplicationUser? user;
        public int? id;
        private readonly SignInManager<ApplicationUser> _sim;
        private IList<Preferences> prefs;
        public Preferences p;

        public ServiceModel(SignInManager<ApplicationUser> sim, UserManager<ApplicationUser> um, WellnessSiteContext con)
        {
            _sim = sim;
            _um = um;
            _context = con;
        }

        public Service Service { get; set; } = default!;

        public bool Bookmarked = false;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
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
                    p = new Preferences("usr-" + _context.Preferences.Count().ToString());
                    _context.Preferences.Add(p);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    string uID = "usr-" + Request.Cookies["user"]!;

                    p = prefs.FirstOrDefault(p => p.UserID == uID)!;

                }
            }

            this.id = id;
            if (id == null || _context.Service == null || _context.Bookmarks == null)
            {
                return NotFound();
            }

            var service = await _context.Service.FirstOrDefaultAsync(m => m.Id == id);
            if (service == null)
            {
                return NotFound();
            }
            else 
            {
                Service = service;
            }

            user = await _um.GetUserAsync(User);
            if(user != null)
            {
                Bookmarked = await _context.Bookmarks.FirstOrDefaultAsync(b => b.SID == service.Id && b.UID == user.Id) != null;
            }
            
            return Page();
        }

        public async Task<IActionResult> OnPostBookmarkAsync(int? id)
        {
            if (id == null || _context.Service == null || _context.Bookmarks == null)
            {
                return NotFound();
            }


            var service = await _context.Service.FirstOrDefaultAsync(m => m.Id == id);
            if (service == null)
            {
                return NotFound();
            }
            else
            {
                Service = service;
            }

            user = await _um.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                Bookmarked = await _context.Bookmarks.FirstOrDefaultAsync(b => b.SID == service.Id && b.UID == user.Id) != null;
            }

            if(Bookmarked)
            {
                var bookmark = await _context.Bookmarks.FirstOrDefaultAsync(b => b.SID == service.Id && b.UID == user.Id);
                if(bookmark != null)
                {
                    _context.Bookmarks.Remove(bookmark);
                    await _context.SaveChangesAsync();
                }
                Bookmarked = false;
            }
            else
            {
                _context.Bookmarks.Add(new Bookmarks { SID = service.Id, UID = user.Id });
                await _context.SaveChangesAsync();
            }

            return Redirect("./Service?id=" + id);
        }
    }
}
