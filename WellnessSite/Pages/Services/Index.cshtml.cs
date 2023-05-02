using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WellnessSite.Data;
using WellnessSite.Models;

namespace WellnessSite.Pages.Services
{
    public class IndexModel : PageModel
    {
        private readonly WellnessSite.Data.WellnessSiteContext _context;

        public IndexModel(WellnessSite.Data.WellnessSiteContext context)
        {
            _context = context;
        }

        public IList<Service> Service { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Service != null)
            {
                Service = await _context.Service.ToListAsync();
            }
        }
    }
}
