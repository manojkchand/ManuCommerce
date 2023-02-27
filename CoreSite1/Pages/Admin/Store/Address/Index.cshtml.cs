using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreSite1.Data;
using CoreSite1.Models;
using Microsoft.AspNetCore.Authorization;

namespace CoreSite1.Pages.Address
{
    [Authorize(Roles = "ThisSiteAdmin")]
    public class IndexModel : PageModel
    {
        private readonly CoreSite1.Data.ApplicationDbContext _context;

        public IndexModel(CoreSite1.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        //public IList<CoreSite1.Models.Address> Address { get;set; }
        public string CurrentFilter { get; set; }
        public PaginatedList<CoreSite1.Models.Address> Address { get; set; }

        public async Task OnGetAsync(string currentFilter, string searchString, int? pageIndex)
        {
            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            CurrentFilter = searchString;

            IQueryable<CoreSite1.Models.Address> addressIQ = from s in _context.Addresses
                                                                    select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                addressIQ = addressIQ.Where(s => s.FirstName.Contains(searchString)
                || s.LastName.Contains(searchString) || s.Phone.Contains(searchString) || s.City.Contains(searchString));
            }

            int pageSize = 10;
            Address = await PaginatedList<CoreSite1.Models.Address>.CreateAsync(
            addressIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
            //Address = await _context.Addresses.ToListAsync();
        }
    }
}
