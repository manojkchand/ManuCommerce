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
    public class MapStocksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MapStocksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/MapStocks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MapStock>>> GetMapStock()
        {
            return await _context.MapStock.ToListAsync();
        }

        // GET: api/MapStocks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MapStock>> GetMapStock(int id)
        {
            var mapStock = await _context.MapStock.Include(e => e.Productlist).Where(a => a.MapImageID == id).FirstOrDefaultAsync();//.FindAsync(id);
            

            if (mapStock == null)
            {
                return NotFound();
            }
            else 
            {
                //var p = await _context.Products.Where(a => a.ProductId == mapStock.ProductId).ToListAsync();
                var p1 = from p in _context.Products
                                                                        join v in _context.Variants on p.ProductId equals v.ProductId
                                                                        where p.ProductId == mapStock.ProductId
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

                //foreach (var v in p)
                //{
                //    v.Variantlist = await _context.Variants.Where(a => a.ProductId == v.ProductId && a.IsDefaulProduct == true).ToListAsync();
                //    // v.UnitInStock = _context.Variants.Where(a => a.ProductId == v.ProductId && a.IsDefaulProduct == true).First().UnitInStock;
                //}

                mapStock.Productlist = p1.ToList();
            }

            return mapStock;
        }

        // PUT: api/MapStocks/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMapStock(int id, MapStock mapStock)
        {
            if (id != mapStock.MapStockID)
            {
                return BadRequest();
            }

            _context.Entry(mapStock).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MapStockExists(id))
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

        // POST: api/MapStocks
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<MapStock>> PostMapStock(MapStock mapStock)
        {
            _context.MapStock.Add(mapStock);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMapStock", new { id = mapStock.MapStockID }, mapStock);
        }

        // DELETE: api/MapStocks/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<MapStock>> DeleteMapStock(int id)
        {
            var mapStock = await _context.MapStock.FindAsync(id);
            if (mapStock == null)
            {
                return NotFound();
            }

            _context.MapStock.Remove(mapStock);
            await _context.SaveChangesAsync();

            return mapStock;
        }

        private bool MapStockExists(int id)
        {
            return _context.MapStock.Any(e => e.MapStockID == id);
        }
    }
}
