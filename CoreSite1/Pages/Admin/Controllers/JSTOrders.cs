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
    [Authorize(Roles = "ThisSiteAdmin")]
    [Route("api/[controller]")]
    [ApiController]
    public class JSTOrders : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public JSTOrders(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/JSTOrders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CoreSite1.Models.Order>>> GetOrders()
        {
            return await _context.Orders.Include(e => e.OrderDetails).ToListAsync();
        }

        // GET: api/JSTOrders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CoreSite1.Models.Order>> GetOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // PUT: api/JSTOrders/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, CoreSite1.Models.Order order)
        {
            if (id != order.OrderId)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
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

        // POST: api/JSTOrders
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<CoreSite1.Models.Order>> PostOrder(CoreSite1.Models.Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.OrderId }, order);
        }

        // DELETE: api/JSTOrders/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CoreSite1.Models.Order>> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return order;
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
