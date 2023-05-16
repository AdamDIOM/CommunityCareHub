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
            p = await UsefulFunctions.GetPreferences(_context, _um, _sim, User, this);

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
                Bookmarked = await _context.Bookmarks.FirstOrDefaultAsync(b => b.ServiceID == service.Id && b.UserID == user.Id) != null;
            }
            
            return Page();
        }

        public async Task<IActionResult> OnPostBookmarkAsync(int? id)
        {

            p = await UsefulFunctions.GetPreferences(_context, _um, _sim, User, this);

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
                Bookmarked = await _context.Bookmarks.FirstOrDefaultAsync(b => b.ServiceID == service.Id && b.UserID == user.Id) != null;
            }

            if(Bookmarked)
            {
                var bookmark = await _context.Bookmarks.FirstOrDefaultAsync(b => b.ServiceID == service.Id && b.UserID == user.Id);
                if(bookmark != null)
                {
                    _context.Bookmarks.Remove(bookmark);
                    await _context.SaveChangesAsync();
                }
                Bookmarked = false;
            }
            else
            {
                _context.Bookmarks.Add(new Bookmarks { ServiceID = service.Id, UserID = user.Id });
                await _context.SaveChangesAsync();
            }

            return Redirect("./Service?id=" + id);
        }
    }
}
