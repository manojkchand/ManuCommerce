using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
//using CoreSite1.Views.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace CoreSite1.Pages.Admin
{
    [Authorize(Roles = "ThisSiteAdmin")]
    public class Dashboard1Model : PageModel
    {
        private readonly CoreSite1.Data.ApplicationDbContext _context;

        public Dashboard1Model(CoreSite1.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public PaginatedList<CoreSite1.Models.ToDoItem> ToDoItem { get; set; }

        public async Task OnGetAsync(int? pageIndex)
        {
            //ToDoItem = await _context.ToDoItem.ToListAsync();
            IQueryable<CoreSite1.Models.ToDoItem> ToDoItemIQ = from s in _context.ToDoItem
                                                                      select s;

            int pageSize = 5;
            ToDoItem = await PaginatedList<CoreSite1.Models.ToDoItem>.CreateAsync(
            ToDoItemIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
        }
    }
}