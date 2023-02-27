using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoreSite1.Data;
using CoreSite1.Models;
using Microsoft.AspNetCore.Authorization;

namespace CoreSite1.Pages.Admin.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class JSTVariants : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public JSTVariants(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/JSTVariants
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CoreSite1.Models.Variant>>> GetVariants()
        {
            return await _context.Variants.ToListAsync();
        }

        // GET: api/JSTVariants/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CoreSite1.Models.Variant>> GetVariant(int id)
        {
            var variant = await _context.Variants.FindAsync(id);

            if (variant == null)
            {
                return NotFound();
            }

            return variant;
        }

        // PUT: api/JSTVariants/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize(Roles = "ThisSiteAdmin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVariant(int id, CoreSite1.Models.Variant variant)
        {
            if (id != variant.VariantId)
            {
                return BadRequest();
            }

            CoreSite1.Models.Variant DBVariant = _context.Variants.Where(e => e.VariantId == variant.VariantId).FirstOrDefault();
            DBVariant.Name = variant.Name;
            //DBpage.URL = CreateURL(DBpage);

            _context.Entry(DBVariant).State = EntityState.Modified;
            // _context.Entry(category).State = EntityState.Modified;
            //_context.Entry(variant).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VariantExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/JSTVariants
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize(Roles = "ThisSiteAdmin")]
        [HttpPost]
        public async Task<ActionResult<CoreSite1.Models.Variant>> PostVariant(CoreSite1.Models.Variant variant)
        {
            variant.AddedDate = DateTime.Now;
            _context.Variants.Add(variant);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVariant", new { id = variant.VariantId }, variant);
        }

        // DELETE: api/JSTVariants/5
        [Authorize(Roles = "ThisSiteAdmin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<CoreSite1.Models.Variant>> DeleteVariant(int id)
        {
            var variant = await _context.Variants.FindAsync(id);
            if (variant == null)
            {
                return NotFound();
            }
            if(variant.IsDefaulProduct != true) 
            { 
            _context.Variants.Remove(variant);
            await _context.SaveChangesAsync();
            }
            return variant;
        }

        private bool VariantExists(int id)
        {
            return _context.Variants.Any(e => e.VariantId == id);
        }
    }
}
