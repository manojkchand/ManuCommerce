using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoreSite1.Data;
using CoreSite1.Models;

namespace CoreSite1.Pages.Admin.Controllers.Map
{
    [Route("api/[controller]")]
    [ApiController]
    public class MapImagesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MapImagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/MapImages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MapImage>>> GetMapImage()
        {
            return await _context.MapImage.ToListAsync();
        }

        // GET: api/MapImages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MapStock>> GetMapImage(int id,string searchString)
        {
            //var mapImage = await _context.MapImage.FindAsync(id);

            //if (mapImage == null)
            //{
            //    return NotFound();
            //}

            //return mapImage;

            IQueryable<CoreSite1.Models.Product> productIQ = from p in _context.Products
                                                             join v in _context.Variants on p.ProductId equals v.ProductId
                                                             //where p.Title == CurrentFilter || p.Brand == currentFilter
                                                             //where v.IsDefaulProduct == true //&& p.Title.Contains(searchString) || v.IsDefaulProduct == true && p.Brand.Contains(searchString)
                                                             select new CoreSite1.Models.Product
                                                             {
                                                                 ProductId = p.ProductId,
                                                                 CategoryId = p.CategoryId,
                                                                 AddedDate = p.AddedDate,
                                                                 AddedBy = p.AddedBy,
                                                                 Title = p.Title,
                                                                 Price = p.Price,
                                                                 Brand = p.Brand,
                                                                 Discount = p.Discount,
                                                                 ProductArtUrl = p.ProductArtUrl,
                                                                 Description = p.Description,
                                                                 StockOfNonVariant = p.StockOfNonVariant,
                                                                 RowVersion = p.RowVersion,

                                                                 UnitInStock = v.UnitInStock,
                                                                 Colour = v.color,
                                                                 Size = v.Size,
                                                                 VariantId = v.VariantId,
                                                                 OptionalImageURL = v.OptionalImageURL,

                                                                 chcekboxAnswer = false
                                                             };//v.VariantId, v.UnitInStock, v.IsDefaulProduct };


            productIQ = productIQ.Where(s => s.Title.Contains(searchString)
                  || s.Brand.Contains(searchString));

            return await _context.MapStock.Where(a => a.ProductId == productIQ.FirstOrDefault().ProductId).FirstOrDefaultAsync();

            
        }

        // PUT: api/MapImages/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMapImage(int id, MapImage mapImage)
        {
            if (id != mapImage.MapImageID)
            {
                return BadRequest();
            }

            _context.Entry(mapImage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MapImageExists(id))
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

        // POST: api/MapImages
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<MapImage>> PostMapImage(MapImage mapImage)
        {
            _context.MapImage.Add(mapImage);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMapImage", new { id = mapImage.MapImageID }, mapImage);
        }

        // DELETE: api/MapImages/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<MapImage>> DeleteMapImage(int id)
        {
            var mapImage = await _context.MapImage.FindAsync(id);
            if (mapImage == null)
            {
                return NotFound();
            }

            _context.MapImage.Remove(mapImage);
            await _context.SaveChangesAsync();

            return mapImage;
        }

        private bool MapImageExists(int id)
        {
            return _context.MapImage.Any(e => e.MapImageID == id);
        }
    }
}
