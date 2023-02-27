using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreSite1.Data;
using CoreSite1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace CoreSite1.Pages.Admin
{
    [Authorize(Roles = "ThisSiteAdmin")]
    public class VariantEditModel : PageModel
    {
        private readonly CoreSite1.Data.ApplicationDbContext _context;

        public VariantEditModel(CoreSite1.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public CoreSite1.Models.Variant Variant { get; set; }
        public IList<CoreSite1.Models.SizeGuide> SizeGuide { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            SizeGuide = await _context.SizeGuide.ToListAsync();

            Variant = await _context.Variants
                .Include(v => v.Product).FirstOrDefaultAsync(m => m.VariantId == id);

            if (Variant == null)
            {
                return NotFound();
            }
           ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Brand");
            //ViewData["SizeGuide"] = new SelectList(_context.SizeGuide, "id", "SizeType");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(IFormCollection values)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            CoreSite1.Models.Variant DBVariant = _context.Variants.Where(e => e.VariantId == Variant.VariantId).FirstOrDefault();
            DBVariant.VariantId= Variant.VariantId;
            DBVariant.Name = Variant.Name;
            //DBVariant.Name = Variant.Name; AddedBy
            //DBVariant.Name = Variant.Name; AddedDate
            DBVariant.Size = Variant.Size;
            DBVariant.UnitInStock = Variant.UnitInStock;
            DBVariant.IsDefaulProduct = Variant.IsDefaulProduct;
            DBVariant.SKU = Variant.SKU;
            DBVariant.OptionalPrice = Variant.OptionalPrice;
            DBVariant.OptionalImageURL = Variant.OptionalImageURL;
            DBVariant.color = Variant.color;
            //values["Page.CategoryId"].ToString();
            //_context.Entry(DBVariant).State = EntityState.Modified;
            _context.Attach(DBVariant).State = EntityState.Modified;
            //_context.Attach(Variant).State = EntityState.Modified;
            //Name AddedBy AddedDate  Size UnitInStock IsDefaulProduct SKU
            //OptionalPrice OptionalImageURL color
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VariantExists(Variant.VariantId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Redirect("~/MyStore/details?id=" + Variant.ProductId + "&vid=" + Variant.VariantId);
        }

        private bool VariantExists(int id)
        {
            return _context.Variants.Any(e => e.VariantId == id);
        }
    }
}
