using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoreSite1.Data;
using CoreSite1.Models;
using CoreSite1.Models.DTO;
using Microsoft.AspNetCore.Authorization;


namespace CoreSite1.Pages.Admin.Service
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class JSTProducts : ControllerBase
    {
        private readonly ApplicationDbContext _context;
      
        public CoreSite1.Models.Variant Variant { get; set; }
        public CoreSite1.Models.Product Product { get; set; }

        public JSTProducts(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/JSTProducts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CoreSite1.Models.Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        // GET: api/JSTProducts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CoreSite1.Models.Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/JSTProducts/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize(Roles = "ThisSiteAdmin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, CoreSite1.Models.Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }

            CoreSite1.Models.Product DBProduct = _context.Products.Where(e => e.ProductId == product.ProductId).FirstOrDefault();
            DBProduct.Title = product.Title;
            //DBpage.URL = CreateURL(DBpage);

            _context.Entry(DBProduct).State = EntityState.Modified;

            //_context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        // POST: api/JSTProducts
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754. Task<ActionResult<Product>>
        [Authorize(Roles = "ThisSiteAdmin")]
        [HttpPost]
        public async Task<JsonResult> PostProduct(CoreSite1.Models.Product product)
        {
            Variant = new CoreSite1.Models.Variant();
            Product = new CoreSite1.Models.Product();
            //product.URL = CreateURL(product);
            product.AddedDate = DateTime.Now;
            //product.AddedBy = User.Identity.Name;
            Product = product;
            _context.Products.Add(Product);
            await _context.SaveChangesAsync();

            Variant.ProductId = Product.ProductId;

            Variant.OptionalPrice = Product.Price;
            Variant.AddedDate = DateTime.Now;
            //Variant.AddedBy = User.Identity.Name;
            Variant.IsDefaulProduct = true;
            Variant.Name = Product.Title;
            Variant.OptionalImageURL = Product.ProductArtUrl;

            _context.Variants.Add(Variant);
            await _context.SaveChangesAsync();
            Product.VariantId = Variant.VariantId;

            var p= CreatedAtAction("GetProduct", new { id = Product.ProductId }, Product);
            
            return new JsonResult(new ProductCategory
            {
                id = Product.ProductId.ToString(),
                vid = Variant.VariantId.ToString(),
                parent = Product.CategoryId.ToString(),
                text = Product.Title
            }
            );
        }

        // DELETE: api/JSTProducts/5
        [Authorize(Roles = "ThisSiteAdmin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<CoreSite1.Models.Product>> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return product;
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
