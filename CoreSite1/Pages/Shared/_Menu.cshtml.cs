using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CoreSite1.Views.Shared
{
    public class _MenuModel : PageModel
    {
        private readonly CoreSite1.Data.ApplicationDbContext _context;

        public _MenuModel(CoreSite1.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<CoreSite1.Models.Category> Category { get; set; }

        public async Task OnGetAsync()
        {
            Category = await _context.Categorys.ToListAsync();
        }
      
    }
}